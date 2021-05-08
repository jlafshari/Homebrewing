using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Services;
using BeerRecipeCore.Styles;
using Xunit;

namespace BeerRecipeCore.Tests.Services
{
    public class RecipeServiceTests
    {
        private readonly RecipeService _recipeService = new();
        
        [Theory]
        [InlineData(85, 15, 8, 5.5f)]
        [InlineData(90, 10, 8, 5.5f)]
        [InlineData(83, 17, 14, 5.0f)]
        public void CanGenerateRecipeWithBaseAndCaramelGrain(int baseMaltProportion, int caramelMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            const float size = 5f;
            var style = new Style("ESB", new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = new List<CommonGrain>
                {
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.MarisOtter,
                        ProportionOfGrist = baseMaltProportion
                    },
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.VictoryMalt,
                        ProportionOfGrist = caramelMaltProportion
                    }
                },
                CommonYeast = RecipeServiceTestHelper.SafAleEnglishAleYeast
            };
            var recipeGenerationInfo = new RecipeGenerationInfo(size, expectedAbv, expectedColorSrm, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, size, style.CommonYeast);
        }
        
        [Theory]
        [InlineData(98, 2, 6, 5.0f)]
        public void CanGeneratePaleAleRecipeWithBaseAndCrystalGrain(int baseMaltProportion, int caramelMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            const float size = 5f;
            var style = new Style("ESB", new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = new List<CommonGrain>
                {
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.TwoRow,
                        ProportionOfGrist = baseMaltProportion
                    },
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.Crystal20LMalt,
                        ProportionOfGrist = caramelMaltProportion
                    }
                },
                CommonYeast = RecipeServiceTestHelper.SafAleEnglishAleYeast
            };
            var recipeGenerationInfo = new RecipeGenerationInfo(size, expectedAbv, expectedColorSrm, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, size, style.CommonYeast);
        }

        [Theory]
        [InlineData(85, 13, 2, 16, 5.5f)]
        [InlineData(85, 10, 5, 8, 5)]
        [InlineData(85, 10, 5, 13, 5.5f)]
        [InlineData(85, 10, 5, 6, 6.0f)]
        public void CanGenerateRecipeWithBaseAndRoastedAndCaramelGrain(int baseMaltProportion, int caramelMaltProportion, int roastedMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            const float size = 5f;
            var style = new Style("ESB", new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = new List<CommonGrain>
                {
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.MarisOtter,
                        ProportionOfGrist = baseMaltProportion
                    },
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.VictoryMalt,
                        ProportionOfGrist = caramelMaltProportion
                    },
                    new()
                    {
                        Fermentable = RecipeServiceTestHelper.ChocolateMalt,
                        ProportionOfGrist = roastedMaltProportion
                    }
                },
                CommonYeast = RecipeServiceTestHelper.SafAleEnglishAleYeast
            };
            var recipeGenerationInfo = new RecipeGenerationInfo(size, expectedAbv, expectedColorSrm, "Brown Ale") { Style = style };
            
            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, size, style.CommonYeast);
        }

        private static void AssertRecipeHasExpectedValues(int expectedColorSrm, float expectedAbv, IRecipe recipe, float size, Yeast.Yeast styleCommonYeast)
        {
            AssertRecipeHasGrains(recipe);

            AssertAllGrainAmountsAreAboveZero(recipe);

            AssertColorIsWithinOneSrm(recipe, size, expectedColorSrm);

            AssertAbvIsEqual(recipe, size, expectedAbv);
            
            Assert.NotNull(recipe.YeastIngredient);
            Assert.Equal(recipe.YeastIngredient.YeastInfo.Name, styleCommonYeast.Name);
        }

        private static void AssertRecipeHasGrains(IRecipe recipe)
        {
            Assert.True(recipe.FermentableIngredients.Count > 0);
        }

        private static void AssertAllGrainAmountsAreAboveZero(IRecipe recipe)
        {
            Assert.True(recipe.FermentableIngredients.All(f => f.Amount > 0));
        }

        private static void AssertColorIsWithinOneSrm(IRecipe recipe, float size, int expectedColorSrm)
        {
            var actualColorSrm = (float) ColorUtility.GetColorInSrm(recipe.FermentableIngredients, size);
            Assert.True(Math.Abs(actualColorSrm - expectedColorSrm) < 1);
        }

        private static void AssertAbvIsEqual(IRecipe recipe, float size, float expectedAbv)
        {
            var originalGravity = AlcoholUtility.GetOriginalGravity(recipe.FermentableIngredients, size, 65);
            var actualAbv = Math.Round(AlcoholUtility.GetAlcoholByVolume(originalGravity, 1.010f), 1);
            Assert.True(Math.Abs(expectedAbv - actualAbv) < 0.1f);
        }
    }
}