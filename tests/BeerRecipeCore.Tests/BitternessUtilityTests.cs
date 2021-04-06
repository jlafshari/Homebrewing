using System.Collections.Generic;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Hops;
using Moq;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class BitternessUtilityTests
    {
        [Fact]
        public void GetBitternessTest()
        {
            var fugglesInRecipe = new Mock<IHopsIngredient>();
            fugglesInRecipe.Setup(h => h.Amount).Returns(1f);
            fugglesInRecipe.Setup(h => h.HopsInfo).Returns(new Hops.Hops("Fuggles", new HopsCharacteristics(4.50f, 2.00f, 0), "Test Notes", "UK"));
            fugglesInRecipe.Setup(h => h.Time).Returns(60);
            
            var goldingsInRecipe = new Mock<IHopsIngredient>();
            goldingsInRecipe.Setup(h => h.Amount).Returns(1f);
            goldingsInRecipe.Setup(h => h.HopsInfo).Returns(new Hops.Hops("Goldings", new HopsCharacteristics(5.00f, 3.50f, 0), "Test Notes", "UK"));
            goldingsInRecipe.Setup(h => h.Time).Returns(15);

            var hopsUsed = new List<IHopsIngredient>() { fugglesInRecipe.Object, goldingsInRecipe.Object };
            int bitterness = BitternessUtility.GetBitterness(hopsUsed, 5f, 1.054f);
            Assert.Equal(23, bitterness);
        }
    }
}
