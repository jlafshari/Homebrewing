using System;
using System.Collections.Generic;
using System.Data.SQLite;
using BeerRecipeCore.Data.Models;
using Utility;

namespace BeerRecipeCore.Data
{
    public static class HopsUtility
    {
        public static IEnumerable<Hops> GetAvailableHopsVarieties(SQLiteConnection connection)
        {
            SQLiteCommand selectHopsCommand = connection.CreateCommand();
            selectHopsCommand.CommandText = "SELECT name, alpha, use, notes, beta, hsi, origin FROM Hops";
            using (SQLiteDataReader reader = selectHopsCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    float alphaAcid = reader.GetFloat(1);
                    string use = reader.GetString(2);
                    string notes = reader.GetString(3);
                    float betaAcid = reader.GetFloat(4);
                    float hsi = reader.GetFloat(5);
                    string origin = reader.GetString(6);

                    HopsCharacteristics characteristics = new HopsCharacteristics(alphaAcid, betaAcid) { Hsi = hsi };
                    yield return new Hops(name, characteristics, use, notes, origin);
                }
            }
        }

        public static HopsIngredientDataModel CreateHopsIngredient(Hops hopsInfo, int recipeId)
        {
            HopsIngredientDataModel hopsIngredient = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                hopsIngredient = CreateHopsIngredient(hopsInfo, recipeId, connection);
                connection.Close();
            }
            return hopsIngredient;
        }

        public static void DeleteHopsIngredient(int hopsIngredientId)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                DeleteHopsIngredient(hopsIngredientId, connection);
                connection.Close();
            }
        }

        internal static HopsIngredientDataModel CreateHopsIngredient(Hops hopsInfo, int recipeId, SQLiteConnection connection)
        {
            using (SQLiteCommand insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO HopsIngredients (amount, time, type, form, hopsInfo) VALUES(0, 0, 'Bittering', 'Leaf', (SELECT id FROM Hops WHERE name = @name))";
                insertCommand.Parameters.AddWithValue("name", hopsInfo.Name);
                insertCommand.ExecuteNonQuery();
            }
            HopsIngredientDataModel hopsIngredient = new HopsIngredientDataModel(hopsInfo, DatabaseUtility.GetLastInsertedRowId(connection));

            using (SQLiteCommand insertJunctionCommand = connection.CreateCommand())
            {
                insertJunctionCommand.CommandText = "INSERT INTO HopsInRecipe (hopsIngredient, recipe) VALUES(@hopsIngredientId, @recipeId)";
                insertJunctionCommand.Parameters.AddWithValue("hopsIngredientId", hopsIngredient.HopsId);
                insertJunctionCommand.Parameters.AddWithValue("recipeId", recipeId);
                insertJunctionCommand.ExecuteNonQuery();
            }
            return hopsIngredient;
        }

        internal static IEnumerable<HopsIngredientDataModel> GetHopsIngredientsForRecipe(int recipeId, SQLiteConnection connection)
        {
            using (SQLiteCommand selectIngredientsCommand = connection.CreateCommand())
            {
                selectIngredientsCommand.CommandText = "SELECT HopsIngredients.id, HopsIngredients.amount, HopsIngredients.time, HopsIngredients.type, HopsIngredients.form, Hops.name, Hops.alpha, Hops.use, Hops.notes, Hops.beta, Hops.hsi, Hops.origin FROM HopsIngredients " +
                    "JOIN HopsInRecipe ON HopsInRecipe.hopsIngredient = HopsIngredients.id AND HopsInRecipe.recipe = @recipeId " +
                    "JOIN Hops ON Hops.id = HopsIngredients.hopsInfo";
                selectIngredientsCommand.Parameters.AddWithValue("recipeId", recipeId);
                using (SQLiteDataReader reader = selectIngredientsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HopsCharacteristics characteristics = new HopsCharacteristics(reader.GetFloat(6), reader.GetFloat(9)) { Hsi = reader.GetFloat(10) };
                        Hops hopsInfo = new Hops(reader.GetString(5), characteristics, reader.GetString(7), reader.GetString(8), reader.GetString(11));
                        yield return new HopsIngredientDataModel(hopsInfo, reader.GetInt32(0))
                        {
                            Amount = reader.GetFloat(1),
                            Time = reader.GetInt32(2),
                            FlavorType = (HopsFlavorType) EnumConverter.Parse(typeof(HopsFlavorType), reader[3].ToString()),
                            Form = (HopsForm) EnumConverter.Parse(typeof(HopsForm), reader[4].ToString())
                        };
                    }
                }
            }
        }

        internal static void UpdateHopsIngredient(HopsIngredientDataModel hopsIngredient, SQLiteConnection connection)
        {
            using (SQLiteCommand updateCommand = connection.CreateCommand())
            {
                updateCommand.CommandText = "UPDATE HopsIngredients SET amount = @amount, time = @time, type = @type, form = @form WHERE id = @id";
                updateCommand.Parameters.AddWithValue("id", hopsIngredient.HopsId);
                updateCommand.Parameters.AddWithValue("amount", hopsIngredient.Amount);
                updateCommand.Parameters.AddWithValue("time", hopsIngredient.Time);
                updateCommand.Parameters.AddWithValue("type", hopsIngredient.FlavorType);
                updateCommand.Parameters.AddWithValue("form", hopsIngredient.Form);
                updateCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteHopsIngredient(int hopsIngredientId, SQLiteConnection connection)
        {
            using (SQLiteCommand deleteIngredientCommand = connection.CreateCommand())
            {
                deleteIngredientCommand.CommandText = "DELETE FROM HopsIngredients WHERE id = @id";
                deleteIngredientCommand.Parameters.AddWithValue("id", hopsIngredientId);
                deleteIngredientCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand deleteJunctionCommand = connection.CreateCommand())
            {
                deleteJunctionCommand.CommandText = "DELETE FROM HopsInRecipe WHERE hopsIngredient = @id";
                deleteJunctionCommand.Parameters.AddWithValue("id", hopsIngredientId);
                deleteJunctionCommand.ExecuteNonQuery();
            }
        }
    }
}
