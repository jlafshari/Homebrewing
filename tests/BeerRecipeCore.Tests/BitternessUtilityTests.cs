using System.Collections.Generic;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Hops;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class BitternessUtilityTests
    {
        [Fact]
        public void GetBitternessTest()
        {
            var fuggles = new Hops.Hops("Fuggles", new HopsCharacteristics(4.50f, 2.00f, 0), "Test Notes", "UK");
            HopsIngredient fugglesInRecipe = new HopsIngredient(fuggles) { Amount = 1f, Form = HopsForm.Leaf, Use = HopsUse.Boil, FlavorType = HopsFlavorType.Bittering, Time = 60 };
            
            var goldings = new Hops.Hops("Goldings", new HopsCharacteristics(5.00f, 3.50f, 0), "Test Notes", "UK");
            HopsIngredient goldingsInRecipe = new HopsIngredient(goldings) { Amount = 1f, Form = HopsForm.Pellet, Use = HopsUse.Boil, FlavorType = HopsFlavorType.Aroma, Time = 15 };

            List<HopsIngredient> hopsUsed = new List<HopsIngredient>() { fugglesInRecipe, goldingsInRecipe };
            int bitterness = BitternessUtility.GetBitterness(hopsUsed, 5f, 1.054f);
            Assert.Equal(23, bitterness);
        }
    }
}
