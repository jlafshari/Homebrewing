using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Fermentables;
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
        private const float RecipeSize = 5f;
        
        [Theory]
        [InlineData(85, 15, 8, 5.5f)]
        [InlineData(90, 10, 8, 5.5f)]
        [InlineData(83, 17, 14, 5.0f)]
        public void CanGenerateRecipeWithBaseAndCaramelGrain(int baseMaltProportion, int caramelMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            var commonGrains = new List<CommonGrain>
            {
                new() { Fermentable = RecipeServiceTestHelper.MarisOtter, ProportionOfGrist = baseMaltProportion },
                new() { Fermentable = RecipeServiceTestHelper.VictoryMalt, ProportionOfGrist = caramelMaltProportion }
            };
            var style = RecipeServiceTestHelper.GetStyle("ESB", commonGrains);
            var recipeGenerationInfo = new RecipeGenerationInfo(RecipeSize, expectedAbv, expectedColorSrm, 0, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, RecipeSize, style.CommonYeast);
        }

        [Theory]
        [InlineData(98, 2, 6, 5.0f)]
        public void CanGeneratePaleAleRecipeWithBaseAndCrystalGrain(int baseMaltProportion, int caramelMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            var commonGrains = new List<CommonGrain>
            {
                new() { Fermentable = RecipeServiceTestHelper.TwoRow, ProportionOfGrist = baseMaltProportion },
                new() { Fermentable = RecipeServiceTestHelper.Crystal20LMalt, ProportionOfGrist = caramelMaltProportion }
            };
            var style = RecipeServiceTestHelper.GetStyle("ESB", commonGrains);
            var recipeGenerationInfo = new RecipeGenerationInfo(RecipeSize, expectedAbv, expectedColorSrm, 0, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, RecipeSize, style.CommonYeast);
        }

        [Theory]
        [InlineData(85, 13, 2, 16, 5.5f)]
        [InlineData(85, 10, 5, 8, 5)]
        [InlineData(85, 10, 5, 13, 5.5f)]
        [InlineData(85, 10, 5, 6, 6.0f)]
        public void CanGenerateRecipeWithBaseAndRoastedAndCaramelGrain(int baseMaltProportion, int caramelMaltProportion, int roastedMaltProportion,
            int expectedColorSrm, float expectedAbv)
        {
            var commonGrains = new List<CommonGrain>
            {
                new() { Fermentable = RecipeServiceTestHelper.MarisOtter, ProportionOfGrist = baseMaltProportion },
                new() { Fermentable = RecipeServiceTestHelper.VictoryMalt, ProportionOfGrist = caramelMaltProportion },
                new() { Fermentable = RecipeServiceTestHelper.ChocolateMalt, ProportionOfGrist = roastedMaltProportion }
            };
            var style = RecipeServiceTestHelper.GetStyle("Brown Ale", commonGrains);
            var recipeGenerationInfo = new RecipeGenerationInfo(RecipeSize, expectedAbv, expectedColorSrm, 0, "Brown Ale") { Style = style };
            
            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);

            AssertRecipeHasExpectedValues(expectedColorSrm, expectedAbv, recipe, RecipeSize, style.CommonYeast);
        }

        [Fact]
        public void CanGenerateRecipe_WithExpectedBitterness_GivenSingleHopAddition()
        {
            var commonGrains = new List<CommonGrain>
            {
                new() { Fermentable = RecipeServiceTestHelper.MarisOtter, ProportionOfGrist = 85 },
                new() { Fermentable = RecipeServiceTestHelper.VictoryMalt, ProportionOfGrist = 15 }
            };
            var commonHops = new List<CommonHop>
            {
                RecipeServiceTestHelper.GetCommonHop(RecipeServiceTestHelper.Fuggles, 60, 100)
            };
            var style = RecipeServiceTestHelper.GetStyle("ESB", commonGrains, commonHops);
            const int expectedIbu = 20;
            var recipeGenerationInfo = new RecipeGenerationInfo(RecipeSize, 5.5f, 8, expectedIbu, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);
            
            AssertRecipeHasExpectedValuesForBitterness(style, recipe, RecipeSize, expectedIbu);
        }
        
        [Fact]
        public void CanGenerateRecipe_WithExpectedBitterness_GivenTwoHopAdditions()
        {
            var commonGrains = new List<CommonGrain>
            {
                new() { Fermentable = RecipeServiceTestHelper.MarisOtter, ProportionOfGrist = 85 },
                new() { Fermentable = RecipeServiceTestHelper.VictoryMalt, ProportionOfGrist = 15 }
            };
            var commonHops = new List<CommonHop>
            {
                RecipeServiceTestHelper.GetCommonHop(RecipeServiceTestHelper.Fuggles, 60, 80),
                RecipeServiceTestHelper.GetCommonHop(RecipeServiceTestHelper.Cascade, 15, 20)
            };
            var style = RecipeServiceTestHelper.GetStyle("ESB", commonGrains, commonHops);
            const int expectedIbu = 30;
            var recipeGenerationInfo = new RecipeGenerationInfo(RecipeSize, 5.5f, 8, expectedIbu, "FSB") { Style = style };

            var recipe = _recipeService.GenerateRecipe(recipeGenerationInfo);
            
            AssertRecipeHasExpectedValuesForBitterness(style, recipe, RecipeSize, expectedIbu);
        }

        [Fact]
        public void CanGetRecipeProjectedOutcome_ReturnsExpectedAbvAndColor()
        {
            var fermentableIngredients = new List<IFermentableIngredient>
            {
                new FermentableIngredient
                {
                    Amount = 8,
                    FermentableInfo = new Fermentable("Pale Malt, Maris Otter",
                        new FermentableCharacteristics(82.5f, 3, 120) { Type = FermentableType.Grain, GravityPoint = 38 }, "Test Notes", "US")
                },
                new FermentableIngredient
                {
                    Amount = 1,
                    FermentableInfo = new Fermentable("Chocolate Malt",
                        new FermentableCharacteristics(60, 350, 0) { Type = FermentableType.Grain, GravityPoint = 28 }, "Test Notes", "US")
                },
                new FermentableIngredient
                {
                    Amount = 0.50f,
                    FermentableInfo = new Fermentable("Caramel/Crystal Malt - 60L",
                        new FermentableCharacteristics(74, 60, 0) { Type = FermentableType.Grain, GravityPoint = 34 }, "Test Notes", "US")
                }
            };

            var projectedOutcome = _recipeService.GetRecipeProjectedOutcome(5.0f, fermentableIngredients, RecipeServiceTestHelper.SafAleEnglishAleYeast, 0);
            Assert.Equal(4.21f, projectedOutcome.Abv);
            Assert.Equal(30, projectedOutcome.ColorSrm);
        }

        private static void AssertRecipeHasExpectedValuesForBitterness(Style style, IRecipe recipe, float size, int expectedIbu)
        {
            Assert.Equal(style.CommonHops.Count, recipe.HopIngredients.Count);
            Assert.True(recipe.HopIngredients.All(f => f.Amount > 0));
            AssertIbuIsEqual(recipe, size, expectedIbu);
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

        private static void AssertIbuIsEqual(IRecipe recipe, float size, int expectedIbu)
        {
            var originalGravity = AlcoholUtility.GetOriginalGravity(recipe.FermentableIngredients, size, 65);
            var actualIbu = BitternessUtility.GetBitterness(recipe.HopIngredients, size, originalGravity);
            Assert.True(Math.Abs(actualIbu - expectedIbu) <= 2);
        }
    }
}