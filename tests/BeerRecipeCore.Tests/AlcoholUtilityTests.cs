using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using Moq;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class AlcoholUtilityTests
    {
        [Fact]
        public void GetAlcoholByVolumeTest()
        {
            float originalGravity = 1.045f;
            float finalGravity = 1.006f;
            float actualAbv = AlcoholUtility.GetAlcoholByVolume(originalGravity, finalGravity);
            Assert.Equal(5.15f, actualAbv);
        }

        [Fact]
        public void GetAlcoholByWeightTest()
        {
            float actualAbw = AlcoholUtility.GetAlcoholByWeight(5.12f);
            Assert.Equal(4.06f, actualAbw);
        }

        [Fact]
        public void GetSpecificAndFinalGravityTest()
        {
            var crystal60InRecipe = new Mock<IFermentableIngredient>();
            crystal60InRecipe.Setup(f => f.Amount).Returns(0.50f);
            crystal60InRecipe.Setup(f => f.FermentableInfo).Returns(new Fermentable("Caramel/Crystal Malt - 60L",
                new FermentableCharacteristics(74, 60, 0) { Type = FermentableType.Grain, GravityPoint = 34 }, "Test Notes", "US"));

            var chocolateMaltInRecipe = new Mock<IFermentableIngredient>();
            chocolateMaltInRecipe.Setup(f => f.Amount).Returns(1);
            chocolateMaltInRecipe.Setup(f => f.FermentableInfo).Returns(new Fermentable("Chocolate Malt",
                new FermentableCharacteristics(60, 350, 0) { Type = FermentableType.Grain, GravityPoint = 28 }, "Test Notes", "US"));

            var marisOtterInRecipe = new Mock<IFermentableIngredient>();
            marisOtterInRecipe.Setup(f => f.Amount).Returns(8);
            marisOtterInRecipe.Setup(f => f.FermentableInfo).Returns(new Fermentable("Pale Malt, Maris Otter",
                new FermentableCharacteristics(82.5f, 3, 120) { Type = FermentableType.Grain, GravityPoint = 38 }, "Test Notes", "US"));

            var fermentablesInRecipe = new List<IFermentableIngredient>() { crystal60InRecipe.Object, chocolateMaltInRecipe.Object, marisOtterInRecipe.Object };
            float actualSpecificGravity = AlcoholUtility.GetOriginalGravity(fermentablesInRecipe, 5, 70);
            Assert.Equal(1.049f, actualSpecificGravity);
            float actualFinalGravity = AlcoholUtility.GetFinalGravity(actualSpecificGravity, 75);
            Assert.Equal(1.012f, actualFinalGravity);
        }

        [Fact]
        public void GetGravityPointTest()
        {
            double specificGravity = 1.037;
            int gravityUnit = AlcoholUtility.GetGravityUnit(specificGravity);
            Assert.Equal(37, gravityUnit);
        }
    }
}
