using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using Moq;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class ColorUtilityTests
    {
        [Fact]
        public void GetColorInSrmTest()
        {
            var crystal60InRecipe = new Mock<IFermentableIngredient>();
            crystal60InRecipe.Setup(f => f.Amount).Returns(0.50f);
            crystal60InRecipe.Setup(f => f.FermentableInfo).Returns(new Fermentable("Caramel/Crystal Malt - 60L",
                new FermentableCharacteristics(74, 60, 0) { Type = FermentableType.Grain }, "Test Notes", "US"));

            var crystal60Color = ColorUtility.GetColorInSrm(new[] { crystal60InRecipe.Object }, 5);
            Assert.Equal(5.1, crystal60Color);

            var marisOtterInRecipe = new Mock<IFermentableIngredient>();
            marisOtterInRecipe.Setup(f => f.Amount).Returns(8);
            marisOtterInRecipe.Setup(f => f.FermentableInfo).Returns(new Fermentable("Pale Malt, Maris Otter",
                new FermentableCharacteristics(82.5f, 3, 120) { Type = FermentableType.Grain }, "Test Notes", "US"));

            var fermentablesUsed = new List<IFermentableIngredient> { marisOtterInRecipe.Object, crystal60InRecipe.Object };
            var colorInSrm = ColorUtility.GetColorInSrm(fermentablesUsed, 5);
            Assert.Equal(7.6, colorInSrm);
        }
    }
}
