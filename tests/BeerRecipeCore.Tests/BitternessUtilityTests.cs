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
            var fugglesInRecipe = new Mock<IHopIngredient>();
            fugglesInRecipe.Setup(h => h.Amount).Returns(1f);
            fugglesInRecipe.Setup(h => h.HopInfo).Returns(new Hop("Fuggles", new HopCharacteristics(4.50f, 2.00f, 0), "Test Notes", "UK"));
            fugglesInRecipe.Setup(h => h.Time).Returns(60);
            
            var goldingsInRecipe = new Mock<IHopIngredient>();
            goldingsInRecipe.Setup(h => h.Amount).Returns(1f);
            goldingsInRecipe.Setup(h => h.HopInfo).Returns(new Hop("Goldings", new HopCharacteristics(5.00f, 3.50f, 0), "Test Notes", "UK"));
            goldingsInRecipe.Setup(h => h.Time).Returns(15);

            var hopsUsed = new List<IHopIngredient> { fugglesInRecipe.Object, goldingsInRecipe.Object };
            var bitterness = BitternessUtility.GetBitterness(hopsUsed, 5f, 1.054f);
            Assert.Equal(23, bitterness);
        }
    }
}
