using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace CreateBeerDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SQLiteConnection connection = new SQLiteConnection(c_connectionString))
            {
                connection.Open();
                
                foreach (string tableCommandString in s_createTableCommands)
                {
                    DbCommand createTablecommand = connection.CreateCommand();
                    createTablecommand.CommandText = tableCommandString;
                    createTablecommand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        const string c_connectionString = @"Data Source=C:\Beer data\Beer.db";
        static readonly string[] s_createTableCommands = new string[]
        {
            "CREATE TABLE Hops (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, alpha NUMERIC, use VARCHAR(10), notes TEXT, beta NUMERIC, hsi NUMERIC, origin VARCHAR(30))",
            "CREATE TABLE Fermentables (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, yield NUMERIC, color NUMERIC, origin VARCHAR(30), notes TEXT, diastaticPower NUMERIC)",
            "CREATE TABLE Yeasts (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, type VARCHAR(30), form VARCHAR(10), amount NUMERIC, amountIsWeight INT, laboratory VARCHAR(30), productId VARCHAR(30), minTemperature NUMERIC, maxTemperature NUMERIC, flocculation VARCHAR(10), attenuation NUMERIC, notes VARCHAR(100), bestFor VARCHAR(100))",
            "CREATE TABLE Styles (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, category VARCHAR(100), categoryNumber INT, styleLetter VARCHAR(1), styleGuide VARCHAR(30), type VARCHAR(10), ogMin NUMERIC, ogMax NUMERIC, fgMin NUMERIC, fgMax NUMERIC, ibuMin NUMERIC, ibuMax NUMERIC, colorMin NUMERIC, colorMax NUMERIC, carbMin NUMERIC, carbMax NUMERIC, abvMin NUMERIC, abvMax NUMERIC, notes TEXT, profile TEXT, ingredients TEXT, examples TEXT)",
            "CREATE TABLE MiscellaneousIngredients (id INTEGER PRIMARY KEY, name VARCHAR(40), version INT, type VARCHAR(10), use VARCHAR(20), useFor VARCHAR(40), notes TEXT)",
            "CREATE TABLE HopsInRecipe (id INTEGER PRIMARY KEY, amount NUMERIC, time NUMERIC, type VARCHAR(10), form VARCHAR(10), hopsInfo INTEGER, FOREIGN KEY(hopsInfo) REFERENCES Hops(id))",
            "CREATE TABLE FermentableInRecipe (id INTEGER PRIMARY KEY, amount NUMERIC, time NUMERIC, type VARCHAR(10), form VARCHAR(10), fermentableInfo INTEGER, FOREIGN KEY(fermentableInfo) REFERENCES Fermentables(id))",
            "CREATE TABLE MiscellaneousIngredientInRecipe (id INTEGER PRIMARY KEY, time NUMERIC, amount NUMERIC, amountIsWeight INT, miscellaneousIngredientInfo INTEGER, FOREIGN KEY(miscellaneousIngredientInfo) REFERENCES MiscellaneousIngredients(id))",
            // TODO: add mash profile table?
            "CREATE TABLE Recipes (id INTEGER PRIMARY KEY, size NUMERIC, boilTime NUMERIC)",
            "CREATE TABLE Batches (id INTEGER PRIMARY KEY, brewerName TEXT, assistantBrewerName TEXT, date TEXT, recipeInfo INTEGER, FOREIGN KEY(recipeInfo) REFERENCES Recipes(id))"
        };
    }
}
