using BeerRecipeCore.Formulas;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class FermentableUtilityTests
    {
        [Fact]
        public void GetOriginalGravityTest()
        {
            var abv = 5.51f;
            var og = FermentableUtility.GetOriginalGravity(abv);
            Assert.Equal(1.052f, og);
        }

        [Fact]
        public void GetPoundsRequiredTest()
        {
            var poundsOfMarisOtterPaleMalt = FermentableUtility.GetPoundsRequired(85, 5, 5.51f, 60, 38);
            Assert.Equal(9.7f, poundsOfMarisOtterPaleMalt);
        }
    }
}