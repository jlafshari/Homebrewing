using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace BeerRecipeCore.Data
{
    public static class RecipeUtility
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

        public static IEnumerable<Style> GetAvailableBeerStyles()
        {
            using (SQLiteConnection connection = new SQLiteConnection(s_databaseConnectionString))
            {
                connection.Open();

                SQLiteCommand stylesCommand = connection.CreateCommand();
                stylesCommand.CommandText = "SELECT Styles.name, Styles.notes, Styles.profile, Styles.ingredients, Styles.examples, StyleCategories.name, StyleCategories.number, StyleCategories.type, StyleClassifications.letter, StyleClassifications.guide FROM Styles " +
                    "JOIN StyleCategories ON Styles.category = StyleCategories.id " +
                    "JOIN StyleClassifications ON Styles.classification = StyleClassifications.id";
                using (SQLiteDataReader reader = stylesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StyleCategory category = new StyleCategory(reader.GetString(5), reader.GetInt32(6), reader.GetString(7));
                        StyleClassification classification = new StyleClassification(reader.GetString(8), reader.GetString(9));

                        string styleName = reader.GetString(0);
                        List<StyleThreshold> thresholds = GetStyleThresholds(styleName, connection).ToList();

                        yield return new Style(styleName, category, classification, thresholds)
                        {
                            Notes = reader.GetString(1),
                            Profile = reader.GetString(2),
                            Ingredients = reader.GetString(3),
                            Examples = reader.GetString(4)
                        };
                    }
                }

                connection.Close();
            }
        }

        private static IEnumerable<StyleThreshold> GetStyleThresholds(string styleName, SQLiteConnection connection)
        {
            SQLiteCommand thresholdCommand = connection.CreateCommand();
            thresholdCommand.CommandText = "SELECT StyleThresholds.value, StyleThresholds.minimum, StyleThresholds.maximum FROM StyleThresholds, Styles, ThresholdsInStyle "
                + "WHERE StyleThresholds.id = ThresholdsInStyle.threshold AND Styles.id = ThresholdsInStyle.style AND Styles.name = @name";
            thresholdCommand.Parameters.AddWithValue("name", styleName);
            using (SQLiteDataReader reader = thresholdCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new StyleThreshold(reader.GetString(0), reader.GetFloat(1), reader.GetFloat(2));
                }
            }
        }

        static readonly string s_databaseConnectionString = "Data Source=" + Properties.Settings.Default.DatabaseLocation;
    }
}
