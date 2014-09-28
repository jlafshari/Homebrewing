using System.Data.SQLite;
using BeerRecipeCore.Data.Models;

namespace BeerRecipeCore.Data
{
    public static class SettingsUtility
    {
        public static SettingsDataModel GetSavedSettings()
        {
            SettingsDataModel settings;

            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            using (SQLiteCommand selectCommand = connection.CreateCommand())
            {
                selectCommand.CommandText = "SELECT * FROM Settings LIMIT 1";
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    reader.Read();
                    settings = new SettingsDataModel(reader.GetInt32(0));
                    settings.RecipeSize = reader.GetFloat(1);
                    settings.BoilTime = reader.GetInt32(2);
                    settings.ExtractionEfficiency = reader.GetFloat(3);
                    settings.YeastWeight = reader.GetFloat(4);
                    settings.HopsAmount = reader.GetFloat(5);
                }
            }
            return settings;
        }

        public static void UpdateSettings(SettingsDataModel settings)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            using (SQLiteCommand updateCommand = connection.CreateCommand())
            {
                updateCommand.CommandText = "UPDATE Settings SET recipeSize = @recipeSize, boilTime = @boilTime, extractionEfficiency = @extractionEfficiency, yeastWeight = @yeastWeight, hopsAmount = @hopsAmount WHERE id = @id";
                updateCommand.Parameters.AddWithValue("id", settings.SettingsId);
                updateCommand.Parameters.AddWithValue("recipeSize", settings.RecipeSize);
                updateCommand.Parameters.AddWithValue("boilTime", settings.BoilTime);
                updateCommand.Parameters.AddWithValue("extractionEfficiency", settings.ExtractionEfficiency);
                updateCommand.Parameters.AddWithValue("yeastWeight", settings.YeastWeight);
                updateCommand.Parameters.AddWithValue("hopsAmount", settings.HopsAmount);
                updateCommand.ExecuteNonQuery();
            }
        }
    }
}
