using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BeerRecipeCore;

namespace CreateBeerDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SQLiteConnection connection = new SQLiteConnection(c_connectionString))
            {
                connection.Open();
                
                // run create table commands
                foreach (string tableCommandString in s_createTableCommands)
                {
                    DbCommand createTablecommand = connection.CreateCommand();
                    createTablecommand.CommandText = tableCommandString;
                    createTablecommand.ExecuteNonQuery();
                }

                // add generic beer data to database
                AddHopsData(connection);
                AddFermentableData(connection);
                AddYeastData(connection);
                AddStylesData(connection);

                connection.Close();
            }
        }

        private static void AddFermentableData(SQLiteConnection connection)
        {
            XDocument fermentables = XDocument.Load(Path.Combine(c_beerDataLocation, @"grain.xml"));
            List<XElement> fermentableEntries = fermentables.Descendants("FERMENTABLE").ToList();
            foreach (XElement fermentableEntry in fermentableEntries)
            {
                string name = fermentableEntry.Element("NAME").Value;
                int version = Convert.ToInt32(fermentableEntry.Element("VERSION").Value);
                string origin = fermentableEntry.Element("ORIGIN").Value;
                string notes = fermentableEntry.Element("NOTES").Value;
                float yield = (float) Convert.ToDouble(fermentableEntry.Element("YIELD").Value);
                float color = (float) Convert.ToDouble(fermentableEntry.Element("COLOR").Value);
                //float diastaticPower = (float) Convert.ToDouble(fermentableEntry.Element("DIASTATIC_POWER").Value);
                float diastaticPowerParsed;
                bool diastaticPowerIsntNull = float.TryParse(fermentableEntry.Element("DIASTATIC_POWER").Value, out diastaticPowerParsed);
                float? diastaticPower = diastaticPowerIsntNull ? (float?) diastaticPowerParsed : null;
                Fermentable fermentableInfo = new Fermentable(name, version, notes, yield, color, origin, diastaticPower);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Fermentables (name, version, yield, color, origin, notes, diastaticPower)"
                    + "VALUES (@name, @version, @yield, @color, @origin, @notes, @diastaticPower)";
                insertCommand.Parameters.AddWithValue("name", fermentableInfo.Name);
                insertCommand.Parameters.AddWithValue("version", fermentableInfo.Version);
                insertCommand.Parameters.AddWithValue("yield", fermentableInfo.Yield);
                insertCommand.Parameters.AddWithValue("color", fermentableInfo.Color);
                insertCommand.Parameters.AddWithValue("origin", fermentableInfo.Origin);
                insertCommand.Parameters.AddWithValue("notes", fermentableInfo.Notes);
                insertCommand.Parameters.AddWithValue("diastaticPower", fermentableInfo.DiastaticPower);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static void AddHopsData(SQLiteConnection connection)
        {
            XDocument hops = XDocument.Load(Path.Combine(c_beerDataLocation, @"hops.xml"));
            List<XElement> hopEntries = hops.Descendants("HOP").ToList();
            foreach (XElement hopEntry in hopEntries)
            {
                string name = hopEntry.Element("NAME").Value;
                int version = Convert.ToInt32(hopEntry.Element("VERSION").Value);
                string origin = hopEntry.Element("ORIGIN").Value;
                float alphaAcid = (float) Convert.ToDouble(hopEntry.Element("ALPHA").Value);
                float betaAcid = (float) Convert.ToDouble(hopEntry.Element("BETA").Value);
                string use = hopEntry.Element("USE").Value;
                string notes = hopEntry.Element("NOTES").Value;
                float hsi = (float) Convert.ToDouble(hopEntry.Element("HSI").Value);
                Hops hopsInfo = new Hops(name, version, alphaAcid, betaAcid, use, notes, hsi, origin);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Hops (name, version, alpha, use, notes, beta, hsi, origin)"
                    + "VALUES (@name, @version, @alpha, @use, @notes, @beta, @hsi, @origin)";
                insertCommand.Parameters.AddWithValue("name", hopsInfo.Name);
                insertCommand.Parameters.AddWithValue("version", hopsInfo.Version);
                insertCommand.Parameters.AddWithValue("alpha", hopsInfo.AlphaAcid);
                insertCommand.Parameters.AddWithValue("use", hopsInfo.Use.SaveToString());
                insertCommand.Parameters.AddWithValue("notes", hopsInfo.Notes);
                insertCommand.Parameters.AddWithValue("beta", hopsInfo.BetaAcid);
                insertCommand.Parameters.AddWithValue("hsi", hopsInfo.Hsi);
                insertCommand.Parameters.AddWithValue("origin", hopsInfo.Origin);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static void AddYeastData(SQLiteConnection connection)
        {
            XDocument yeasts = XDocument.Load(Path.Combine(c_beerDataLocation, @"yeast.xml"));
            List<XElement> yeastEntries = yeasts.Descendants("YEAST").ToList();
            foreach (XElement yeastEntry in yeastEntries)
            {
                string name = yeastEntry.Element("NAME").Value;
                int version = Convert.ToInt32(yeastEntry.Element("VERSION").Value);
                string type = yeastEntry.Element("TYPE").Value;
                string form = yeastEntry.Element("FORM").Value;
                float amount = (float) Convert.ToDouble(yeastEntry.Element("AMOUNT").Value);
                int amountIsWeight = bool.Parse(yeastEntry.Element("AMOUNT_IS_WEIGHT").Value) ? 1 : 0;
                string laboratory = yeastEntry.Element("LABORATORY").Value;
                string productId = yeastEntry.Element("PRODUCT_ID").Value;
                float minTemperature = (float) Convert.ToDouble(yeastEntry.Element("MIN_TEMPERATURE").Value);
                float maxTemperature = (float) Convert.ToDouble(yeastEntry.Element("MAX_TEMPERATURE").Value);
                string flocculation = yeastEntry.Element("FLOCCULATION").Value;
                float attenuation = (float) Convert.ToDouble(yeastEntry.Element("ATTENUATION").Value);
                string notes = yeastEntry.Element("NOTES").Value;
                string bestFor = yeastEntry.Element("BEST_FOR").Value;

                Yeast yeastInfo = new Yeast(name, version, notes, type, form, amount, amountIsWeight == 0, laboratory, productId, minTemperature, maxTemperature,
                    flocculation, attenuation, bestFor);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Yeasts (name, version, type, form, amount, amountIsWeight, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes, bestFor)"
                    + "VALUES (@name, @version, @type, @form, @amount, @amountIsWeight, @laboratory, @productId, @minTemperature, @maxTemperature, @flocculation, @attenuation, @notes, @bestFor)";
                insertCommand.Parameters.AddWithValue("name", yeastInfo.Name);
                insertCommand.Parameters.AddWithValue("version", yeastInfo.Version);
                insertCommand.Parameters.AddWithValue("type", yeastInfo.Type.SaveToString());
                insertCommand.Parameters.AddWithValue("form", yeastInfo.Form.SaveToString());
                insertCommand.Parameters.AddWithValue("amount", yeastInfo.Amount);
                insertCommand.Parameters.AddWithValue("amountIsWeight", yeastInfo.AmountIsWeight ? 1 : 0);
                insertCommand.Parameters.AddWithValue("laboratory", yeastInfo.Laboratory);
                insertCommand.Parameters.AddWithValue("productId", yeastInfo.ProductId);
                insertCommand.Parameters.AddWithValue("minTemperature", yeastInfo.MinTemperature);
                insertCommand.Parameters.AddWithValue("maxTemperature", yeastInfo.MaxTemperature);
                insertCommand.Parameters.AddWithValue("flocculation", yeastInfo.Flocculation.SaveToString());
                insertCommand.Parameters.AddWithValue("attenuation", yeastInfo.Attenuation);
                insertCommand.Parameters.AddWithValue("notes", yeastInfo.Notes);
                insertCommand.Parameters.AddWithValue("bestFor", yeastInfo.BestFor);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static void AddStylesData(SQLiteConnection connection)
        {
            XDocument styles = XDocument.Load(Path.Combine(c_beerDataLocation, "style.xml"));
            List<XElement> styleEntries = styles.Descendants("STYLE").ToList();
            foreach (XElement styleEntry in styleEntries)
            {
                string name = styleEntry.Element("NAME").Value;
                int version = Convert.ToInt32(styleEntry.Element("VERSION").Value);
                string category = styleEntry.Element("CATEGORY").Value;
                int categoryNumber = Convert.ToInt32(styleEntry.Element("CATEGORY_NUMBER").Value);
                string styleLetter = styleEntry.Element("STYLE_LETTER").Value;
                string styleGuide = styleEntry.Element("STYLE_GUIDE").Value;
                string type = styleEntry.Element("TYPE").Value;
                float ogMin = (float) Convert.ToDouble(styleEntry.Element("OG_MIN").Value);
                float ogMax = (float) Convert.ToDouble(styleEntry.Element("OG_MAX").Value);
                float fgMin = (float) Convert.ToDouble(styleEntry.Element("FG_MIN").Value);
                float fgMax = (float) Convert.ToDouble(styleEntry.Element("FG_MAX").Value);
                float ibuMin = (float) Convert.ToDouble(styleEntry.Element("IBU_MIN").Value);
                float ibuMax = (float) Convert.ToDouble(styleEntry.Element("IBU_MAX").Value);
                float colorMin = (float) Convert.ToDouble(styleEntry.Element("COLOR_MIN").Value);
                float colorMax = (float) Convert.ToDouble(styleEntry.Element("COLOR_MAX").Value);
                float carbMin = (float) Convert.ToDouble(styleEntry.Element("CARB_MIN").Value);
                float carbMax = (float) Convert.ToDouble(styleEntry.Element("CARB_MAX").Value);
                float abvMin = (float) Convert.ToDouble(styleEntry.Element("ABV_MIN").Value);
                float abvMax = (float) Convert.ToDouble(styleEntry.Element("ABV_MAX").Value);
                string notes = styleEntry.Element("NOTES").Value;
                string profile = styleEntry.Element("PROFILE").Value;
                string ingredients = styleEntry.Element("INGREDIENTS").Value;
                string examples = styleEntry.Element("EXAMPLES").Value;
            }
        }

        const string c_beerDataLocation = @"C:\Beer data";
        const string c_connectionString = @"Data Source=" + c_beerDataLocation + @"\Beer.db";
        static readonly string[] s_createTableCommands = new string[]
        {
            "CREATE TABLE Hops (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, alpha NUMERIC, use VARCHAR(10), notes TEXT, beta NUMERIC, hsi NUMERIC, origin VARCHAR(30))",
            "CREATE TABLE Fermentables (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, yield NUMERIC, color NUMERIC, origin VARCHAR(30), notes TEXT, diastaticPower NUMERIC)",
            "CREATE TABLE Yeasts (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, type VARCHAR(30), form VARCHAR(10), amount NUMERIC, amountIsWeight INT, laboratory VARCHAR(30), productId VARCHAR(30), minTemperature NUMERIC, maxTemperature NUMERIC, flocculation VARCHAR(10), attenuation NUMERIC, notes VARCHAR(100), bestFor VARCHAR(100))",
            "CREATE TABLE Styles (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, category VARCHAR(100), categoryNumber INT, styleLetter VARCHAR(1), styleGuide VARCHAR(30), type VARCHAR(10), ogMin NUMERIC, ogMax NUMERIC, fgMin NUMERIC, fgMax NUMERIC, ibuMin NUMERIC, ibuMax NUMERIC, colorMin NUMERIC, colorMax NUMERIC, carbMin NUMERIC, carbMax NUMERIC, abvMin NUMERIC, abvMax NUMERIC, notes TEXT, profile TEXT, ingredients TEXT, examples TEXT)",
            "CREATE TABLE MiscellaneousIngredients (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, type VARCHAR(10), use VARCHAR(20), useFor VARCHAR(40), notes TEXT)",
            
            "CREATE TABLE HopsIngredients (id INTEGER PRIMARY KEY, amount NUMERIC, time NUMERIC, type VARCHAR(10), form VARCHAR(10), hopsInfo INTEGER, FOREIGN KEY(hopsInfo) REFERENCES Hops(id))",
            "CREATE TABLE FermentableIngredients (id INTEGER PRIMARY KEY, amount NUMERIC, time NUMERIC, type VARCHAR(10), form VARCHAR(10), fermentableInfo INTEGER, FOREIGN KEY(fermentableInfo) REFERENCES Fermentables(id))",
            "CREATE TABLE MiscellaneousIngredientInRecipe (id INTEGER PRIMARY KEY, time NUMERIC, amount NUMERIC, amountIsWeight INT, miscellaneousIngredientInfo INTEGER, FOREIGN KEY(miscellaneousIngredientInfo) REFERENCES MiscellaneousIngredients(id))",
            
            // junction tables
            "CREATE TABLE HopsInRecipe (id INTEGER PRIMARY KEY, hopsIngredient INTEGER, recipe INTEGER, FOREIGN KEY(hopsIngredient) REFERENCES HopsIngredients(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",
            "CREATE TABLE FermentablesInRecipe (id INTEGER PRIMARY KEY, fermentableIngredient INTEGER, recipe INTEGER, FOREIGN KEY(fermentableIngredient) REFERENCES FermentableIngredients(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",
            "CREATE TABLE MiscellaneousIngredientsInRecipe (id INTEGER PRIMARY KEY, miscellaneousIngredient INTEGER, recipe INTEGER, FOREIGN KEY(miscellaneousIngredient) REFERENCES MiscellaneousIngredientInRecipe(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",

            // TODO: add mash profile table?
            "CREATE TABLE Recipes (id INTEGER PRIMARY KEY, size NUMERIC, boilTime NUMERIC, beerStyleInfo INTEGER, FOREIGN KEY(beerStyleInfo) REFERENCES Styles(id))",
            "CREATE TABLE Batches (id INTEGER PRIMARY KEY, brewerName TEXT, assistantBrewerName TEXT, date TEXT, recipeInfo INTEGER, FOREIGN KEY(recipeInfo) REFERENCES Recipes(id))"
        };
    }
}
