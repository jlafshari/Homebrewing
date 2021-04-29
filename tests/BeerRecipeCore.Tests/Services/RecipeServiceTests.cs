using System;
using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Services;
using BeerRecipeCore.Styles;
using Xunit;

namespace BeerRecipeCore.Tests.Services
{
    public class RecipeServiceTests
    {
        private readonly RecipeService _recipeService = new();
        
        [Fact]
        public void CanGenerateRecipeWithBaseAndCaramelGrain()
        {
            const float size = 5f;
            const int expectedColorSrm = 8;
            const float expectedAbv = 5.5f;
            var style = new Style("ESB", new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = new List<CommonGrain>
                {
                    new()
                    {
                        Fermentable = MarisOtter,
                        ProportionOfGrist = 85
                    },
                    new()
                    {
                        Fermentable = VictoryMalt,
                        ProportionOfGrist = 15
                    }
                }
            };

            var recipe = _recipeService.GenerateRecipe(size, style, expectedAbv, expectedColorSrm, "FSB");

            Assert.Equal(style.CommonGrains.Count, recipe.FermentableIngredients.Count);
            Assert.Equal(style.CommonGrains[0].Fermentable, recipe.FermentableIngredients[0].FermentableInfo);
            
            AssertColorIsWithinOneSrm(recipe, size, expectedColorSrm);

            AssertAbvIsEqual(recipe, size, expectedAbv);
        }

        [Fact]
        public void CanGenerateRecipeWithBaseAndRoastedAndCaramelGrain()
        {
            const float size = 5f;
            const int expectedColorSrm = 16;
            const float expectedAbv = 5.5f;
            var style = new Style("ESB", new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = new List<CommonGrain>
                {
                    new()
                    {
                        Fermentable = MarisOtter,
                        ProportionOfGrist = 85
                    },
                    new()
                    {
                        Fermentable = VictoryMalt,
                        ProportionOfGrist = 13
                    },
                    new()
                    {
                        Fermentable = ChocolateMalt,
                        ProportionOfGrist = 2
                    }
                }
            };
            
            var recipe = _recipeService.GenerateRecipe(size, style, expectedAbv, expectedColorSrm, "Brown Ale");

            Assert.Equal(style.CommonGrains.Count, recipe.FermentableIngredients.Count);
            Assert.Equal(style.CommonGrains[0].Fermentable, recipe.FermentableIngredients[0].FermentableInfo);
            
            AssertColorIsWithinOneSrm(recipe, size, expectedColorSrm);

            AssertAbvIsEqual(recipe, size, expectedAbv);
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
            Assert.Equal(expectedAbv, actualAbv);
        }

        private static Fermentable VictoryMalt =>
            new("Victory Malt",
                new FermentableCharacteristics(73f, 25f, 50f) { GravityPoint = 34, MaltCategory = MaltCategory.Caramel }, "", "");

        private static Fermentable MarisOtter =>
            new("Maris Otter",
                new FermentableCharacteristics(82.5f, 3f, 120f) { GravityPoint = 38, MaltCategory = MaltCategory.Base }, "", "");

        private static Fermentable ChocolateMalt =>
            new("Chocolate Malt",
                new FermentableCharacteristics(60f, 350f, 0) { GravityPoint = 28, MaltCategory = MaltCategory.Roasted }, "", "");
    }
}