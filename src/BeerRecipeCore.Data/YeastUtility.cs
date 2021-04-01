using System.Collections.Generic;
using System.Data.SQLite;
using BeerRecipeCore.Data.Models;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Data
{
    public static class YeastUtility
    {
        public static IEnumerable<Yeast.Yeast> GetAvailableYeasts(SQLiteConnection connection)
        {
            SQLiteCommand selectYeastCommand = connection.CreateCommand();
            selectYeastCommand.CommandText = "SELECT name, type, form, laboratory, productId, minTemperature, maxTemperature, flocculation, attenuation, notes FROM Yeasts";
            using (SQLiteDataReader reader = selectYeastCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string form = reader.GetString(2);
                    string laboratory = reader.GetString(3);
                    string productId = reader.GetString(4);
                    float minTemperature = reader.GetFloat(5);
                    float maxTemperature = reader.GetFloat(6);
                    string flocculation = reader.GetString(7);
                    float attenuation = reader.GetFloat(8);
                    string notes = reader.GetString(9);

                    YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form)
                    {
                        Attenuation = attenuation,
                        MinTemperature = minTemperature,
                        MaxTemperature = maxTemperature
                    };
                    yield return new Yeast.Yeast(name, characteristics, notes, laboratory, productId);
                }
            }
        }

        public static YeastIngredientDataModel CreateYeastIngredient()
        {
            YeastIngredientDataModel yeastIngredient = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                yeastIngredient = CreateYeastIngredient(connection);
                connection.Close();
            }
            return yeastIngredient;
        }

        internal static YeastIngredientDataModel CreateYeastIngredient(SQLiteConnection connection)
        {
            using (SQLiteCommand insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO YeastIngredients (weight, volume, yeastInfo) VALUES(0, 0, 0)";
                insertCommand.ExecuteNonQuery();
            }
            return new YeastIngredientDataModel(null, DatabaseUtility.GetLastInsertedRowId(connection));
        }

        internal static YeastIngredientDataModel GetYeastIngredientForRecipe(int recipeId, SQLiteConnection connection)
        {
            using (SQLiteCommand selectIngredientsCommand = connection.CreateCommand())
            {
                selectIngredientsCommand.CommandText = "SELECT YeastIngredients.id, YeastIngredients.weight, YeastIngredients.volume, Yeasts.id, Yeasts.name, Yeasts.type, Yeasts.form, Yeasts.laboratory, Yeasts.productId, Yeasts.minTemperature, Yeasts.maxTemperature, Yeasts.flocculation, Yeasts.attenuation, Yeasts.notes FROM YeastIngredients " +
                    "JOIN Recipes ON Recipes.yeastIngredientInfo = YeastIngredients.id AND Recipes.id = @recipeId " +
                    "JOIN Yeasts ON Yeasts.id = YeastIngredients.yeastInfo " +
                    "LIMIT 1";
                selectIngredientsCommand.Parameters.AddWithValue("recipeId", recipeId);
                using (SQLiteDataReader reader = selectIngredientsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        YeastCharacteristics characteristics = new YeastCharacteristics(reader.GetString(5), reader.GetString(11), reader.GetString(6))
                        {
                            Attenuation = reader.GetFloat(12),
                            MinTemperature = reader.GetFloat(9),
                            MaxTemperature = reader.GetFloat(10)
                        };
                        var yeastInfo = new Yeast.Yeast(reader.GetString(4), characteristics, reader.GetString(13), reader.GetString(7), reader.GetString(8));
                        return new YeastIngredientDataModel(yeastInfo, reader.GetInt32(0)) { Volume = reader.GetFloat(2), Weight = reader.GetFloat(1) };
                    }
                }
            }
            return null;
        }

        internal static void UpdateYeastIngredient(YeastIngredientDataModel yeastIngredient, SQLiteConnection connection)
        {
            using (SQLiteCommand updateCommand = connection.CreateCommand())
            {
                updateCommand.CommandText = "UPDATE YeastIngredients SET weight = @weight, volume = @volume, yeastInfo = (SELECT id FROM Yeasts WHERE name = @yeastName) " +
                    "WHERE id = @id";
                updateCommand.Parameters.AddWithValue("id", yeastIngredient.YeastIngredientId);
                updateCommand.Parameters.AddWithValue("weight", yeastIngredient.Weight);
                updateCommand.Parameters.AddWithValue("volume", yeastIngredient.Volume);
                updateCommand.Parameters.AddWithValue("yeastName", yeastIngredient.YeastInfo != null ? yeastIngredient.YeastInfo.Name: "");
                updateCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteYeastIngredient(int yeastIngredientId, SQLiteConnection connection)
        {
            using (SQLiteCommand deleteCommand = connection.CreateCommand())
            {
                deleteCommand.CommandText = "DELETE FROM YeastIngredients WHERE id = @id";
                deleteCommand.Parameters.AddWithValue("id", yeastIngredientId);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
