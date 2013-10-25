using System.Collections.Generic;
using System.Data.SQLite;

namespace BeerRecipeCore.Data
{
    public static class IngredientUtility
    {
        public static IEnumerable<IngredientTypeBase> GetAvailableIngredients()
        {
            List<IngredientTypeBase> ingredients = new List<IngredientTypeBase>();
            using (SQLiteConnection connection = new SQLiteConnection(s_databaseConnectionString))
            {
                connection.Open();

                ingredients.AddRange(HopsUtility.GetAvailableHopsVarieties(connection));
                ingredients.AddRange(FermentableUtility.GetAvailableFermentables(connection));
                ingredients.AddRange(YeastUtility.GetAvailableYeasts(connection));

                connection.Close();
            }

            return ingredients;
        }

        static string s_databaseConnectionString = string.Concat("Data Source=", Properties.Settings.Default.DatabaseLocation);
    }
}
