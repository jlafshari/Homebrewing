using System.Collections.Generic;
using System.Data.SQLite;
using BeerRecipeCore.Data.Models;
using BeerRecipeCore.Hops;
using Utility;

namespace BeerRecipeCore.Data
{
    public static class HopsUtility
    {
        public static IEnumerable<Hops.Hops> GetAvailableHopsVarieties(SQLiteConnection connection)
        {
            SQLiteCommand selectHopsCommand = connection.CreateCommand();
            selectHopsCommand.CommandText = "SELECT name, alpha, notes, beta, hsi, origin FROM Hops";
            using SQLiteDataReader reader = selectHopsCommand.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                float alphaAcid = reader.GetFloat(1);
                string notes = reader.GetString(2);
                float betaAcid = reader.GetFloat(3);
                float hsi = reader.GetFloat(4);
                string origin = reader.GetString(5);

                var characteristics = new HopsCharacteristics(alphaAcid, betaAcid, hsi);
                yield return new Hops.Hops(name, characteristics, notes, origin);
            }
        }

        public static HopsIngredientDataModel CreateHopsIngredient(Hops.Hops hopsInfo, int recipeId)
        {
            using SQLiteConnection connection = DatabaseUtility.GetNewConnection();
            var hopsIngredient = CreateHopsIngredient(hopsInfo, recipeId, connection);
            connection.Close();
            return hopsIngredient;
        }

        public static void DeleteHopsIngredient(int hopsIngredientId)
        {
            using SQLiteConnection connection = DatabaseUtility.GetNewConnection();
            DeleteHopsIngredient(hopsIngredientId, connection);
            connection.Close();
        }

        internal static HopsIngredientDataModel CreateHopsIngredient(Hops.Hops hopsInfo, int recipeId, SQLiteConnection connection)
        {
            using var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = "INSERT INTO HopsIngredients (amount, time, type, form, use, hopsInfo) VALUES(0, 0, 'Bittering', 'Leaf', 'Boil', (SELECT id FROM Hops WHERE name = @name))";
            insertCommand.Parameters.AddWithValue("name", hopsInfo.Name);
            insertCommand.ExecuteNonQuery();
            HopsIngredientDataModel hopsIngredient = new HopsIngredientDataModel(hopsInfo, DatabaseUtility.GetLastInsertedRowId(connection));

            using var insertJunctionCommand = connection.CreateCommand();
            insertJunctionCommand.CommandText = "INSERT INTO HopsInRecipe (hopsIngredient, recipe) VALUES(@hopsIngredientId, @recipeId)";
            insertJunctionCommand.Parameters.AddWithValue("hopsIngredientId", hopsIngredient.HopsId);
            insertJunctionCommand.Parameters.AddWithValue("recipeId", recipeId);
            insertJunctionCommand.ExecuteNonQuery();
            return hopsIngredient;
        }

        internal static IEnumerable<HopsIngredientDataModel> GetHopsIngredientsForRecipe(int recipeId, SQLiteConnection connection)
        {
            using var selectIngredientsCommand = connection.CreateCommand();
            selectIngredientsCommand.CommandText = "SELECT HopsIngredients.id, HopsIngredients.amount, HopsIngredients.time, HopsIngredients.type, HopsIngredients.form, Hops.name, Hops.alpha, HopsIngredients.use, Hops.notes, Hops.beta, Hops.hsi, Hops.origin, HopsIngredients.dryHopTime FROM HopsIngredients " +
                "JOIN HopsInRecipe ON HopsInRecipe.hopsIngredient = HopsIngredients.id AND HopsInRecipe.recipe = @recipeId " +
                "JOIN Hops ON Hops.id = HopsIngredients.hopsInfo";
            selectIngredientsCommand.Parameters.AddWithValue("recipeId", recipeId);
            using SQLiteDataReader reader = selectIngredientsCommand.ExecuteReader();
            while (reader.Read())
            {
                HopsCharacteristics characteristics = new HopsCharacteristics(reader.GetFloat(6), reader.GetFloat(9), reader.GetFloat(10));
                var hopsInfo = new Hops.Hops(reader.GetString(5), characteristics, reader.GetString(8), reader.GetString(11));
                string dryHopTimeValue = reader[12].ToString();
                int? dryHopTime = dryHopTimeValue.IsNullOrEmpty() ? null : (int?)int.Parse(dryHopTimeValue);
                yield return new HopsIngredientDataModel(hopsInfo, reader.GetInt32(0))
                {
                    Amount = reader.GetFloat(1),
                    Time = reader.GetInt32(2),
                    FlavorType = EnumConverter.Parse<HopsFlavorType>(reader[3].ToString()),
                    Form = EnumConverter.Parse<HopsForm>(reader[4].ToString()),
                    Use = EnumConverter.Parse<HopsUse>(reader.GetString(7)),
                    DryHopTime = dryHopTime
                };
            }
        }

        internal static void UpdateHopsIngredient(HopsIngredientDataModel hopsIngredient, SQLiteConnection connection)
        {
            using var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = "UPDATE HopsIngredients SET amount = @amount, time = @time, type = @type, form = @form, use = @use, dryHopTime = @dryHopTime WHERE id = @id";
            updateCommand.Parameters.AddWithValue("id", hopsIngredient.HopsId);
            updateCommand.Parameters.AddWithValue("amount", hopsIngredient.Amount);
            updateCommand.Parameters.AddWithValue("time", hopsIngredient.Time);
            updateCommand.Parameters.AddWithValue("type", hopsIngredient.FlavorType);
            updateCommand.Parameters.AddWithValue("form", hopsIngredient.Form);
            updateCommand.Parameters.AddWithValue("use", hopsIngredient.Use);
            updateCommand.Parameters.AddWithValue("dryHopTime", hopsIngredient.DryHopTime);
            updateCommand.ExecuteNonQuery();
        }

        internal static void DeleteHopsIngredient(int hopsIngredientId, SQLiteConnection connection)
        {
            using var deleteIngredientCommand = connection.CreateCommand();
            deleteIngredientCommand.CommandText = "DELETE FROM HopsIngredients WHERE id = @id";
            deleteIngredientCommand.Parameters.AddWithValue("id", hopsIngredientId);
            deleteIngredientCommand.ExecuteNonQuery();
            using var deleteJunctionCommand = connection.CreateCommand();
            deleteJunctionCommand.CommandText = "DELETE FROM HopsInRecipe WHERE hopsIngredient = @id";
            deleteJunctionCommand.Parameters.AddWithValue("id", hopsIngredientId);
            deleteJunctionCommand.ExecuteNonQuery();
        }
    }
}
