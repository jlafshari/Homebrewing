using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using BeerRecipeCore.Data.Models;

namespace BeerRecipeCore.Data
{
    public static class RecipeUtility
    {
        public static IEnumerable<IngredientTypeBase> GetAvailableIngredients()
        {
            List<IngredientTypeBase> ingredients = new List<IngredientTypeBase>();
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                ingredients.AddRange(HopsUtility.GetAvailableHopsVarieties(connection));
                ingredients.AddRange(FermentableUtility.GetAvailableFermentables(connection));
                ingredients.AddRange(YeastUtility.GetAvailableYeasts(connection));

                connection.Close();
            }
            return ingredients;
        }

        public static IEnumerable<Style> GetAvailableBeerStyles()
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
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

        public static IEnumerable<RecipeDataModel> GetSavedRecipes(IList<Style> availableBeerStyles)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                using (SQLiteCommand getRecipesCommand = connection.CreateCommand())
                {
                    getRecipesCommand.CommandText = "SELECT Recipes.id, Recipes.size, Recipes.boilTime, Recipes.name, Styles.name FROM Recipes " +
                        "LEFT JOIN Styles ON Styles.id = Recipes.beerStyleInfo";
                    using (SQLiteDataReader reader = getRecipesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string styleName = reader[4].ToString();
                            Style recipeStyle = availableBeerStyles.FirstOrDefault(style => style.Name == styleName);
                            int recipeId = reader.GetInt32(0);
                            RecipeDataModel recipe = new RecipeDataModel(recipeId)
                            {
                                Size = reader.GetFloat(1),
                                BoilTime = reader.GetInt32(2),
                                Name = reader.GetString(3),
                                Style = recipeStyle,
                                YeastIngredient = YeastUtility.GetYeastIngredientForRecipe(recipeId, connection)
                            };

                            foreach (HopsIngredientDataModel hopsIngredient in HopsUtility.GetHopsIngredientsForRecipe(recipeId, connection))
                                recipe.HopsIngredients.Add(hopsIngredient);

                            foreach (FermentableIngredientDataModel fermentableIngredient in FermentableUtility.GetFermentableIngredientsForRecipe(recipeId, connection))
                                recipe.FermentableIngredients.Add(fermentableIngredient);

                            yield return recipe;
                        }
                    }
                }
                connection.Close();
            }
        }

        public static void SaveRecipe(RecipeDataModel recipe)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                using (SQLiteCommand updateRecipeCommand = connection.CreateCommand())
                {
                    updateRecipeCommand.CommandText = "UPDATE Recipes SET size = @size, boilTime = @boilTime, name = @name, yeastIngredientInfo = @yeastIngredientInfo, beerStyleInfo = (SELECT id FROM Styles WHERE name = @beerStyleName) " +
                        "WHERE id = @id";
                    updateRecipeCommand.Parameters.AddWithValue("id", recipe.RecipeId);
                    updateRecipeCommand.Parameters.AddWithValue("size", recipe.Size);
                    updateRecipeCommand.Parameters.AddWithValue("boilTime", recipe.BoilTime);
                    updateRecipeCommand.Parameters.AddWithValue("name", recipe.Name);
                    updateRecipeCommand.Parameters.AddWithValue("yeastIngredientInfo", recipe.YeastIngredient!= null ? ((YeastIngredientDataModel) recipe.YeastIngredient).YeastIngredientId : 0);
                    updateRecipeCommand.Parameters.AddWithValue("beerStyleName", recipe.Style != null ? recipe.Style.Name : "");
                    updateRecipeCommand.ExecuteNonQuery();
                    if (recipe.YeastIngredient != null)
                        YeastUtility.UpdateYeastIngredient((YeastIngredientDataModel) recipe.YeastIngredient, connection);

                    foreach (FermentableIngredientDataModel fermentableIngredient in recipe.FermentableIngredients)
                        FermentableUtility.UpdateFermentableIngredient(fermentableIngredient, connection);

                    foreach (HopsIngredientDataModel hopsIngredient in recipe.HopsIngredients)
                        HopsUtility.UpdateHopsIngredient(hopsIngredient, connection);
                }
                connection.Close();
            }
        }

        public static RecipeDataModel CreateRecipe()
        {
            RecipeDataModel recipe = null;
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                YeastIngredientDataModel yeastIngredient = YeastUtility.CreateYeastIngredient(connection);
                using (SQLiteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO Recipes (size, boilTime, name, beerStyleInfo, yeastIngredientInfo, mashProfileInfo) VALUES(0, 0, '', 0, @yeastIngredientInfo, 0)";
                    insertCommand.Parameters.AddWithValue("yeastIngredientInfo", yeastIngredient.YeastIngredientId);
                    insertCommand.ExecuteNonQuery();
                }
                recipe = new RecipeDataModel(DatabaseUtility.GetLastInsertedRowId(connection)) { YeastIngredient = yeastIngredient };
                connection.Close();
            }
            return recipe;
        }

        public static void DeleteRecipe(RecipeDataModel recipe)
        {
            using (SQLiteConnection connection = DatabaseUtility.GetNewConnection())
            {
                if (recipe.YeastIngredient != null)
                    YeastUtility.DeleteYeastIngredient(((YeastIngredientDataModel) recipe.YeastIngredient).YeastIngredientId, connection);

                foreach (FermentableIngredientDataModel fermentableIngredient in recipe.FermentableIngredients)
                    FermentableUtility.DeleteFermentableIngredient(fermentableIngredient.FermentableId, connection);

                foreach (HopsIngredientDataModel hopsIngredient in recipe.HopsIngredients)
                    HopsUtility.DeleteHopsIngredient(hopsIngredient.HopsId, connection);

                using (SQLiteCommand deleteRecipeCommand = connection.CreateCommand())
                {
                    deleteRecipeCommand.CommandText = "DELETE FROM Recipes WHERE id = @id";
                    deleteRecipeCommand.Parameters.AddWithValue("id", recipe.RecipeId);
                    deleteRecipeCommand.ExecuteNonQuery();
                }
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
    }
}
