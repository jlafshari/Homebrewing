using BeerRecipeCore.Data.Models;

namespace BeerRecipeCore.Data
{
    public static class SettingsUtility
    {
        public static SettingsDataModel GetSavedSettings()
        {
            using var connection = DatabaseUtility.GetNewConnection();
            using var selectCommand = connection.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM Settings LIMIT 1";
            using var reader = selectCommand.ExecuteReader();
            reader.Read();
            return new SettingsDataModel(reader.GetInt32(0))
            {
                RecipeSize = reader.GetFloat(1),
                BoilTime = reader.GetInt32(2),
                ExtractionEfficiency = reader.GetFloat(3),
                YeastWeight = reader.GetFloat(4),
                HopsAmount = reader.GetFloat(5)
            };
        }

        public static void UpdateSettings(SettingsDataModel settings)
        {
            using var connection = DatabaseUtility.GetNewConnection();
            using var updateCommand = connection.CreateCommand();
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
