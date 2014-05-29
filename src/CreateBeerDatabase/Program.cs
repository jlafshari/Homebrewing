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
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
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
                    AddDefaultSettings(connection);
                    transaction.Commit();
                }
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
                XElement maltCategoryElement = fermentableEntry.Element("malt-category");
                if (maltCategoryElement != null)
                    fermentableInfo.Characteristics.MaltCategory = (MaltCategory?) EnumConverter.Parse(typeof(MaltCategory), maltCategoryElement.Value);

                SQLiteCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Fermentables (name, yield, yieldByWeight, color, origin, notes, diastaticPower, type, maltCategory, gravityPoint)"
                    + "VALUES (@name, @yield, @yieldByWeight, @color, @origin, @notes, @diastaticPower, @type, @maltCategory, @gravityPoint)";
                insertCommand.Parameters.AddWithValue("name", fermentableInfo.Name);
                insertCommand.Parameters.AddWithValue("yield", fermentableInfo.Characteristics.Yield);
                insertCommand.Parameters.AddWithValue("yieldByWeight", fermentableInfo.Characteristics.YieldByWeight);
                insertCommand.Parameters.AddWithValue("color", fermentableInfo.Characteristics.Color);
                insertCommand.Parameters.AddWithValue("origin", fermentableInfo.Origin);
                insertCommand.Parameters.AddWithValue("notes", fermentableInfo.Notes);
                insertCommand.Parameters.AddWithValue("diastaticPower", fermentableInfo.Characteristics.DiastaticPower);
                insertCommand.Parameters.AddWithValue("type", fermentableInfo.Characteristics.Type.SaveToString());
                insertCommand.Parameters.AddWithValue("maltCategory", fermentableInfo.Characteristics.MaltCategory != null ? fermentableInfo.Characteristics.MaltCategory.SaveToString() : null);
                insertCommand.Parameters.AddWithValue("gravityPoint", fermentableInfo.Characteristics.GravityPoint);
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
                insertCommand.CommandText = "INSERT INTO Hops (name, alpha, notes, beta, hsi, origin)"
                    + "VALUES (@name, @alpha, @notes, @beta, @hsi, @origin)";
                insertCommand.Parameters.AddWithValue("name", hopsInfo.Name);
                insertCommand.Parameters.AddWithValue("alpha", hopsInfo.Characteristics.AlphaAcid);
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
                insertCommand.CommandText = "INSERT INTO Yeasts (name, type, form, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes)"
                    + "VALUES (@name, @type, @form, @laboratory, @productId, @minTemperature, @maxTemperature, @flocculation, @attenuation, @notes)";
                insertCommand.Parameters.AddWithValue("name", yeastInfo.Name);
                insertCommand.Parameters.AddWithValue("type", yeastInfo.Characteristics.Type.SaveToString());
                insertCommand.Parameters.AddWithValue("form", yeastInfo.Characteristics.Form.SaveToString());
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
            List<StyleCategory> categoriesAdded = new List<StyleCategory>();
            List<StyleClassification> classificationsAdded = new List<StyleClassification>();
            foreach (XElement styleEntry in styleEntries)
            {
                Style styleInfo = BeerXmlImportUtility.GetStyle(styleEntry);

                StyleCategory category = styleInfo.Category;
                if (!categoriesAdded.Any(cat => cat.Name == category.Name && cat.Number == category.Number && cat.Type == category.Type))
                {
                    categoriesAdded.Add(category);

                    // insert into StyleCategories table
                    SQLiteCommand categoryInsertCommand = connection.CreateCommand();
                    categoryInsertCommand.CommandText = "INSERT INTO StyleCategories (name, number, type) VALUES(@name, @number, @type)";
                    categoryInsertCommand.Parameters.AddWithValue("name", category.Name);
                    categoryInsertCommand.Parameters.AddWithValue("number", category.Number);
                    categoryInsertCommand.Parameters.AddWithValue("type", category.Type.SaveToString());
                    categoryInsertCommand.ExecuteNonQuery();
                }

                StyleClassification classification = styleInfo.Classification;
                if (!classificationsAdded.Any(cls => cls.StyleGuide == classification.StyleGuide && cls.StyleLetter == classification.StyleLetter))
                {
                    classificationsAdded.Add(classification);

                    // insert into StyleClassifications table
                    SQLiteCommand classificationInsertCommand = connection.CreateCommand();
                    classificationInsertCommand.CommandText = "INSERT INTO StyleClassifications (letter, guide) VALUES(@letter, @guide)";
                    classificationInsertCommand.Parameters.AddWithValue("letter", classification.StyleLetter);
                    classificationInsertCommand.Parameters.AddWithValue("guide", classification.StyleGuide);
                    classificationInsertCommand.ExecuteNonQuery();
                }

                // insert into Styles table
                SQLiteCommand styleInsertCommand = connection.CreateCommand();
                styleInsertCommand.CommandText = "INSERT INTO Styles (name, category, classification, notes, profile, ingredients, examples)"
                    + " VALUES(@name, (SELECT id FROM StyleCategories WHERE name = @categoryName), "
                    + "(SELECT id FROM StyleClassifications WHERE letter = @styleLetter AND guide = @styleGuide), @notes, @profile, @ingredients, @examples)";
                styleInsertCommand.Parameters.AddWithValue("name", styleInfo.Name);
                styleInsertCommand.Parameters.AddWithValue("categoryName", styleInfo.Category.Name);
                styleInsertCommand.Parameters.AddWithValue("styleLetter", styleInfo.Classification.StyleLetter);
                styleInsertCommand.Parameters.AddWithValue("styleGuide", styleInfo.Classification.StyleGuide);
                styleInsertCommand.Parameters.AddWithValue("notes", styleInfo.Notes);
                styleInsertCommand.Parameters.AddWithValue("profile", styleInfo.Profile);
                styleInsertCommand.Parameters.AddWithValue("ingredients", styleInfo.Ingredients);
                styleInsertCommand.Parameters.AddWithValue("examples", styleInfo.Examples);
                styleInsertCommand.ExecuteNonQuery();

                // style thresholds
                foreach (StyleThreshold threshold in styleInfo.Thresholds)
                {
                    SQLiteCommand thresholdInsertCommand = connection.CreateCommand();
                    thresholdInsertCommand.CommandText = "INSERT INTO StyleThresholds (value, minimum, maximum) VALUES(@value, @minimum, @maximum)";
                    thresholdInsertCommand.Parameters.AddWithValue("value", threshold.Value);
                    thresholdInsertCommand.Parameters.AddWithValue("minimum", threshold.Minimum);
                    thresholdInsertCommand.Parameters.AddWithValue("maximum", threshold.Maximum);
                    thresholdInsertCommand.ExecuteNonQuery();

                    // insert into junction table
                    SQLiteCommand thresholdJunctionInsertCommand = connection.CreateCommand();
                    thresholdJunctionInsertCommand.CommandText = "INSERT INTO ThresholdsInStyle (threshold, style) VALUES("
                        + "(SELECT id FROM StyleThresholds WHERE value = @thresholdValue AND minimum = @minimum AND maximum = @maximum),"
                        + "(SELECT id FROM Styles WHERE name = @name))";
                    thresholdJunctionInsertCommand.Parameters.AddWithValue("thresholdValue", threshold.Value);
                    thresholdJunctionInsertCommand.Parameters.AddWithValue("minimum", threshold.Minimum);
                    thresholdJunctionInsertCommand.Parameters.AddWithValue("maximum", threshold.Maximum);
                    thresholdJunctionInsertCommand.Parameters.AddWithValue("name", styleInfo.Name);
                    thresholdJunctionInsertCommand.ExecuteNonQuery();
                }
            }
        }

        private static void AddDefaultSettings(SQLiteConnection connection)
        {
            using (SQLiteCommand insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO Settings (recipeSize, boilTime, extractionEfficiency) VALUES (@recipeSize, @boilTime, @extractionEfficiency)";
                insertCommand.Parameters.AddWithValue("recipeSize", RecipeDefaultSettings.Size);
                insertCommand.Parameters.AddWithValue("boilTime", RecipeDefaultSettings.BoilTime);
                insertCommand.Parameters.AddWithValue("extractionEfficiency", RecipeDefaultSettings.ExtractionEfficiency);
                insertCommand.ExecuteNonQuery();
            }
        }

        const string c_beerDataLocation = @"C:\Beer data";
        const string c_connectionString = @"Data Source=" + c_beerDataLocation + @"\Beer.db";
        static readonly string[] s_createTableCommands = new string[]
        {
            "CREATE TABLE Hops (id INTEGER PRIMARY KEY, name VARCHAR(40), alpha NUMERIC, notes TEXT, beta NUMERIC, hsi NUMERIC, origin VARCHAR(30))",
            "CREATE TABLE Fermentables (id INTEGER PRIMARY KEY, name VARCHAR(40), yield NUMERIC, yieldByWeight NUMERIC, color NUMERIC, origin VARCHAR(30), notes TEXT, diastaticPower NUMERIC, type VARCHAR(10), maltCategory VARCHAR(10), gravityPoint INTEGER)",
            "CREATE TABLE Yeasts (id INTEGER PRIMARY KEY, name VARCHAR(40), type VARCHAR(30), form VARCHAR(10), laboratory VARCHAR(30), productId VARCHAR(30), minTemperature NUMERIC, maxTemperature NUMERIC, flocculation VARCHAR(10), attenuation NUMERIC, notes VARCHAR(100))",
            "CREATE TABLE Styles (id INTEGER PRIMARY KEY, name VARCHAR(40), category INT, classification INT, notes TEXT, profile TEXT, ingredients TEXT, examples TEXT, FOREIGN KEY(category) REFERENCES StyleCategories(id), FOREIGN KEY(classification) REFERENCES StyleClassifications(id))",
            "CREATE TABLE StyleCategories (id INTEGER PRIMARY KEY, name VARCHAR(40), number INT, type VARCHAR(10))",
            "CREATE TABLE StyleClassifications (id INTEGER PRIMARY KEY, letter VARCHAR(1), guide VARCHAR(30))",
            "CREATE TABLE StyleThresholds (id INTEGER PRIMARY KEY, value VARCHAR(10), minimum NUMERIC, maximum NUMERIC)",
            "CREATE TABLE MiscellaneousIngredients (id INTEGER PRIMARY KEY, name VARCHAR(40), type VARCHAR(10), use VARCHAR(20), useFor VARCHAR(40), notes TEXT)",
            "CREATE TABLE MashSteps (id INTEGER PRIMARY KEY, type VARCHAR(20), infuseAmount NUMERIC, targetTemperature INTEGER, duration INTEGER, infusionWaterTemperature INTEGER)",
            "CREATE TABLE MashProfiles (id INTEGER PRIMARY KEY, grainStartingTemperature INTEGER, waterToGrainRatio NUMERIC)",
            "CREATE TABLE GravityReadings (id INTEGER PRIMARY KEY, specificGravity NUMERIC, date TEXT)",
            
            "CREATE TABLE HopsIngredients (id INTEGER PRIMARY KEY, amount NUMERIC, time NUMERIC, dryHopTime NUMERIC, type VARCHAR(10), form VARCHAR(10), use VARCHAR(10), hopsInfo INTEGER, FOREIGN KEY(hopsInfo) REFERENCES Hops(id))",
            "CREATE TABLE FermentableIngredients (id INTEGER PRIMARY KEY, amount NUMERIC, fermentableInfo INTEGER, FOREIGN KEY(fermentableInfo) REFERENCES Fermentables(id))",
            "CREATE TABLE YeastIngredients (id INTEGER PRIMARY KEY, weight NUMERIC, volume NUMERIC, yeastInfo INTEGER, FOREIGN KEY(yeastInfo) REFERENCES Yeasts(id))",
            "CREATE TABLE MiscellaneousIngredientInRecipe (id INTEGER PRIMARY KEY, time NUMERIC, amount NUMERIC, amountIsWeight INT, miscellaneousIngredientInfo INTEGER, FOREIGN KEY(miscellaneousIngredientInfo) REFERENCES MiscellaneousIngredients(id))",

            "CREATE TABLE Settings (id INTEGER PRIMARY KEY, recipeSize NUMERIC, boilTime INTEGER, extractionEfficiency NUMERIC)",
            
            // junction tables
            "CREATE TABLE HopsInRecipe (id INTEGER PRIMARY KEY, hopsIngredient INTEGER, recipe INTEGER, FOREIGN KEY(hopsIngredient) REFERENCES HopsIngredients(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",
            "CREATE TABLE FermentablesInRecipe (id INTEGER PRIMARY KEY, fermentableIngredient INTEGER, recipe INTEGER, FOREIGN KEY(fermentableIngredient) REFERENCES FermentableIngredients(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",
            "CREATE TABLE MiscellaneousIngredientsInRecipe (id INTEGER PRIMARY KEY, miscellaneousIngredient INTEGER, recipe INTEGER, FOREIGN KEY(miscellaneousIngredient) REFERENCES MiscellaneousIngredientInRecipe(id), FOREIGN KEY(recipe) REFERENCES Recipes(id))",
            "CREATE TABLE ThresholdsInStyle (id INTEGER PRIMARY KEY, threshold INT, style INT, FOREIGN KEY(threshold) REFERENCES StyleThresholds(id), FOREIGN KEY(style) REFERENCES Styles(id))",
            "CREATE TABLE MashStepsInProfile (id INTEGER PRIMARY KEY, mashStep INTEGER, mashProfile INTEGER, FOREIGN KEY(mashStep) REFERENCES MashSteps, FOREIGN KEY(mashProfile) REFERENCES MashProfiles)",
            "CREATE TABLE GravityReadingsInBatch (id INTEGER PRIMARY KEY, gravityReading INTEGER, batch INTEGER, FOREIGN KEY(gravityReading) REFERENCES GravityReadings, FOREIGN KEY(batch) REFERENCES Batches)",

            "CREATE TABLE Recipes (id INTEGER PRIMARY KEY, size NUMERIC NOT NULL, boilTime INTEGER NOT NULL, name TEXT NOT NULL, yeastIngredientInfo INTEGER, beerStyleInfo INTEGER, mashProfileInfo INTEGER, FOREIGN KEY(yeastIngredientInfo) REFERENCES YeastIngredients(id), FOREIGN KEY(beerStyleInfo) REFERENCES Styles(id), FOREIGN KEY(mashProfileInfo) REFERENCES MashProfiles(id))",
            "CREATE TABLE Batches (id INTEGER PRIMARY KEY, brewerName TEXT, assistantBrewerName TEXT, brewingDate TEXT, recipeInfo INTEGER, FOREIGN KEY(recipeInfo) REFERENCES Recipes(id))"
        };
    }
}
