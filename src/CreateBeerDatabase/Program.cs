using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BeerRecipeCore;
using BeerRecipeCore.BeerXml;
using Utility;

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
                Fermentable fermentableInfo = BeerXmlImportUtility.GetFermentable(fermentableEntry);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Fermentables (name, yield, color, origin, notes, diastaticPower)"
                    + "VALUES (@name, @yield, @color, @origin, @notes, @diastaticPower)";
                insertCommand.Parameters.AddWithValue("name", fermentableInfo.Name);
                insertCommand.Parameters.AddWithValue("yield", fermentableInfo.Characteristics.Yield);
                insertCommand.Parameters.AddWithValue("color", fermentableInfo.Characteristics.Color);
                insertCommand.Parameters.AddWithValue("origin", fermentableInfo.Origin);
                insertCommand.Parameters.AddWithValue("notes", fermentableInfo.Notes);
                insertCommand.Parameters.AddWithValue("diastaticPower", fermentableInfo.Characteristics.DiastaticPower);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static void AddHopsData(SQLiteConnection connection)
        {
            XDocument hops = XDocument.Load(Path.Combine(c_beerDataLocation, @"hops.xml"));
            List<XElement> hopEntries = hops.Descendants("HOP").ToList();
            foreach (XElement hopEntry in hopEntries)
            {
                Hops hopsInfo = BeerXmlImportUtility.GetHops(hopEntry);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Hops (name, alpha, use, notes, beta, hsi, origin)"
                    + "VALUES (@name, @alpha, @use, @notes, @beta, @hsi, @origin)";
                insertCommand.Parameters.AddWithValue("name", hopsInfo.Name);
                insertCommand.Parameters.AddWithValue("alpha", hopsInfo.Characteristics.AlphaAcid);
                insertCommand.Parameters.AddWithValue("use", hopsInfo.Use.SaveToString());
                insertCommand.Parameters.AddWithValue("notes", hopsInfo.Notes);
                insertCommand.Parameters.AddWithValue("beta", hopsInfo.Characteristics.BetaAcid);
                insertCommand.Parameters.AddWithValue("hsi", hopsInfo.Characteristics.Hsi);
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
                Yeast yeastInfo = BeerXmlImportUtility.GetYeast(yeastEntry);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Yeasts (name, type, form, amount, amountIsWeight, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes)"
                    + "VALUES (@name, @type, @form, @amount, @amountIsWeight, @laboratory, @productId, @minTemperature, @maxTemperature, @flocculation, @attenuation, @notes)";
                insertCommand.Parameters.AddWithValue("name", yeastInfo.Name);
                insertCommand.Parameters.AddWithValue("type", yeastInfo.Characteristics.Type.SaveToString());
                insertCommand.Parameters.AddWithValue("form", yeastInfo.Characteristics.Form.SaveToString());
                insertCommand.Parameters.AddWithValue("amount", yeastInfo.Amount);
                insertCommand.Parameters.AddWithValue("amountIsWeight", yeastInfo.AmountIsWeight ? 1 : 0);
                insertCommand.Parameters.AddWithValue("laboratory", yeastInfo.Laboratory);
                insertCommand.Parameters.AddWithValue("productId", yeastInfo.ProductId);
                insertCommand.Parameters.AddWithValue("minTemperature", yeastInfo.Characteristics.MinTemperature);
                insertCommand.Parameters.AddWithValue("maxTemperature", yeastInfo.Characteristics.MaxTemperature);
                insertCommand.Parameters.AddWithValue("flocculation", yeastInfo.Characteristics.Flocculation.SaveToString());
                insertCommand.Parameters.AddWithValue("attenuation", yeastInfo.Characteristics.Attenuation);
                insertCommand.Parameters.AddWithValue("notes", yeastInfo.Notes);
                insertCommand.ExecuteNonQuery();
            }
        }

        private static void AddStylesData(SQLiteConnection connection)
        {
            XDocument styles = XDocument.Load(Path.Combine(c_beerDataLocation, "style.xml"));
            List<XElement> styleEntries = styles.Descendants("STYLE").ToList();
            foreach (XElement styleEntry in styleEntries)
            {
                Style styleInfo = BeerXmlImportUtility.GetStyle(styleEntry);
                // TODO: add to database
            }
        }

        const string c_beerDataLocation = @"C:\Beer data";
        const string c_connectionString = @"Data Source=" + c_beerDataLocation + @"\Beer.db";
        static readonly string[] s_createTableCommands = new string[]
        {
            "CREATE TABLE Hops (id INTEGER PRIMARY KEY, name VARCHAR(40), alpha NUMERIC, use VARCHAR(10), notes TEXT, beta NUMERIC, hsi NUMERIC, origin VARCHAR(30))",
            "CREATE TABLE Fermentables (id INTEGER PRIMARY KEY, name VARCHAR(40), yield NUMERIC, color NUMERIC, origin VARCHAR(30), notes TEXT, diastaticPower NUMERIC)",
            "CREATE TABLE Yeasts (id INTEGER PRIMARY KEY, name VARCHAR(40), type VARCHAR(30), form VARCHAR(10), amount NUMERIC, amountIsWeight INT, laboratory VARCHAR(30), productId VARCHAR(30), minTemperature NUMERIC, maxTemperature NUMERIC, flocculation VARCHAR(10), attenuation NUMERIC, notes VARCHAR(100))",
            "CREATE TABLE Styles (id INTEGER PRIMARY KEY, name VARCHAR(40), category VARCHAR(100), categoryNumber INT, styleLetter VARCHAR(1), styleGuide VARCHAR(30), type VARCHAR(10), ogMin NUMERIC, ogMax NUMERIC, fgMin NUMERIC, fgMax NUMERIC, ibuMin NUMERIC, ibuMax NUMERIC, colorMin NUMERIC, colorMax NUMERIC, carbMin NUMERIC, carbMax NUMERIC, abvMin NUMERIC, abvMax NUMERIC, notes TEXT, profile TEXT, ingredients TEXT, examples TEXT)",
            "CREATE TABLE MiscellaneousIngredients (id INTEGER PRIMARY KEY, name VARCHAR(40), type VARCHAR(10), use VARCHAR(20), useFor VARCHAR(40), notes TEXT)",
            
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
