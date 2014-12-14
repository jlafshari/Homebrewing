using System;
using System.Collections.Generic;
using System.Data.SQLite;
using BeerRecipeCore.Data.Models;
using Utility;

namespace BeerRecipeCore.Data
{
    public static class FermentableUtility
    {
        public static IEnumerable<Fermentable> GetAvailableFermentables(SQLiteConnection connection)
        {
            SQLiteCommand selectFermentablesCommand = connection.CreateCommand();
            selectFermentablesCommand.CommandText = "SELECT name, yield, yieldByWeight, color, origin, notes, diastaticPower, type, maltCategory, gravityPoint FROM Fermentables";
            using (SQLiteDataReader reader = selectFermentablesCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string yieldValue = reader[1].ToString();
                    float? yield = !yieldValue.IsNullOrEmpty() ? (float?) float.Parse(yieldValue) : null;
                    string yieldByWeightValue = reader[2].ToString();
                    float? yieldByWeight = !yieldByWeightValue.IsNullOrEmpty() ? (float?) float.Parse(yieldByWeightValue) : null;
                    float color = reader.GetFloat(3);
                    string origin = reader.GetString(4);
                    string notes = reader.GetString(5);
                    string diastaticPowerValue = reader[6].ToString();
                    float? diastaticPower = !diastaticPowerValue.IsNullOrEmpty() ? (float?) float.Parse(diastaticPowerValue) : null;
                    FermentableType type = EnumConverter.Parse<FermentableType>(reader.GetString(7));
                    string maltCategoryValue = reader[8].ToString();
                    MaltCategory? maltCategory = !maltCategoryValue.IsNullOrEmpty() ? (MaltCategory?) EnumConverter.Parse<MaltCategory>(maltCategoryValue) : null;
                    int gravityPoint = reader.GetInt32(9);

                    FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower) {
                        YieldByWeight = yieldByWeight, Type = type, MaltCategory = maltCategory, GravityPoint = gravityPoint };
                    yield return new Fermentable(name, characteristics, notes, origin);
                }
            }
        }

        public static FermentableIngredientDataModel CreateFermentableIngredient(Fermentable fermentableInfo, int recipeId)
        {
            FermentableIngredientDataModel fermentableIngredient = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                fermentableIngredient = CreateFermentableIngredient(fermentableInfo, recipeId, connection);
                connection.Close();
            }
            return fermentableIngredient;
        }

        public static void DeleteFermentableIngredient(int fermentableIngredientId)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                DeleteFermentableIngredient(fermentableIngredientId, connection);
                connection.Close();
            }
        }

        internal static FermentableIngredientDataModel CreateFermentableIngredient(Fermentable fermentableInfo, int recipeId, SQLiteConnection connection)
        {
            using (SQLiteCommand insertIngredientCommand = connection.CreateCommand())
            {
                insertIngredientCommand.CommandText = "INSERT INTO FermentableIngredients (amount, fermentableInfo) VALUES(0, (SELECT id FROM Fermentables WHERE name = @name))";
                insertIngredientCommand.Parameters.AddWithValue("name", fermentableInfo.Name);
                insertIngredientCommand.ExecuteNonQuery();
            }
            FermentableIngredientDataModel fermentableIngredient = new FermentableIngredientDataModel(fermentableInfo, DatabaseUtility.GetLastInsertedRowId(connection));

            using (SQLiteCommand insertJunctionCommand = connection.CreateCommand())
            {
                insertJunctionCommand.CommandText = "INSERT INTO FermentablesInRecipe (fermentableIngredient, recipe) VALUES(@fermentableIngredientId, @recipeId)";
                insertJunctionCommand.Parameters.AddWithValue("fermentableIngredientId", fermentableIngredient.FermentableId);
                insertJunctionCommand.Parameters.AddWithValue("recipeId", recipeId);
                insertJunctionCommand.ExecuteNonQuery();
            }
            return fermentableIngredient;
        }

        internal static IEnumerable<FermentableIngredientDataModel> GetFermentableIngredientsForRecipe(int recipeId, SQLiteConnection connection)
        {
            using (SQLiteCommand selectIngredientsCommand = connection.CreateCommand())
            {
                selectIngredientsCommand.CommandText = "SELECT FermentableIngredients.id, FermentableIngredients.amount, Fermentables.name, Fermentables.yield, Fermentables.yieldByWeight, Fermentables.color, Fermentables.origin, Fermentables.notes, Fermentables.diastaticPower, Fermentables.type, Fermentables.maltCategory, Fermentables.gravityPoint FROM FermentableIngredients " +
                    "JOIN FermentablesInRecipe ON FermentablesInRecipe.fermentableIngredient = FermentableIngredients.id AND FermentablesInRecipe.recipe = @recipeId " +
                    "JOIN Fermentables ON Fermentables.id = FermentableIngredients.fermentableInfo";
                selectIngredientsCommand.Parameters.AddWithValue("recipeId", recipeId);
                using (SQLiteDataReader reader = selectIngredientsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string yieldValue = reader[3].ToString();
                        float? yield = !yieldValue.IsNullOrEmpty() ? (float?) float.Parse(yieldValue) : null;
                        string yieldByWeightValue = reader[4].ToString();
                        float? yieldByWeight = !yieldByWeightValue.IsNullOrEmpty() ? (float?) float.Parse(yieldByWeightValue) : null;
                        string diastaticPowerValue = reader[8].ToString();
                        float? diastaticPower = !diastaticPowerValue.IsNullOrEmpty() ? (float?) float.Parse(diastaticPowerValue) : null;
                        string maltCategoryValue = reader[10].ToString();
                        MaltCategory? maltCategory = !maltCategoryValue.IsNullOrEmpty() ? (MaltCategory?) EnumConverter.Parse<MaltCategory>(maltCategoryValue) : null;
                        FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, reader.GetFloat(5), diastaticPower)
                        {
                            GravityPoint = reader.GetInt32(11),
                            Type = EnumConverter.Parse<FermentableType>(reader.GetString(9)),
                            MaltCategory = maltCategory,
                            YieldByWeight = yieldByWeight
                        };
                        Fermentable fermentableInfo = new Fermentable(reader.GetString(2), characteristics, reader.GetString(7), reader.GetString(6));
                        yield return new FermentableIngredientDataModel(fermentableInfo, reader.GetInt32(0)) { Amount = reader.GetFloat(1) };
                    }
                }
            }
        }

        internal static void UpdateFermentableIngredient(FermentableIngredientDataModel fermentableIngredient, SQLiteConnection connection)
        {
            using (SQLiteCommand updateCommand = connection.CreateCommand())
            {
                updateCommand.CommandText = "UPDATE FermentableIngredients SET amount = @amount WHERE id = @id";
                updateCommand.Parameters.AddWithValue("id", fermentableIngredient.FermentableId);
                updateCommand.Parameters.AddWithValue("amount", fermentableIngredient.Amount);
                updateCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteFermentableIngredient(int fermentableIngredientId, SQLiteConnection connection)
        {
            using (SQLiteCommand deleteIngredientCommand = connection.CreateCommand())
            {
                deleteIngredientCommand.CommandText = "DELETE FROM FermentableIngredients WHERE id = @id";
                deleteIngredientCommand.Parameters.AddWithValue("id", fermentableIngredientId);
                deleteIngredientCommand.ExecuteNonQuery();
            }
            using (SQLiteCommand deleteJunctionCommand = connection.CreateCommand())
            {
                deleteJunctionCommand.CommandText = "DELETE FROM FermentablesInRecipe WHERE fermentableIngredient = @id";
                deleteJunctionCommand.Parameters.AddWithValue("id", fermentableIngredientId);
                deleteJunctionCommand.ExecuteNonQuery();
            }
        }
    }
}
