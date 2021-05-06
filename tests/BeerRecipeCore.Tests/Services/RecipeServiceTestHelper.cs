using BeerRecipeCore.Fermentables;

namespace BeerRecipeCore.Tests.Services
{
    internal static class RecipeServiceTestHelper
    {
        internal static Fermentable VictoryMalt => new("Victory Malt",
                new FermentableCharacteristics(73f, 25f, 50f) { GravityPoint = 34, MaltCategory = MaltCategory.Caramel }, "", "");

        internal static Fermentable MarisOtter => new("Maris Otter",
                new FermentableCharacteristics(82.5f, 3f, 120f) { GravityPoint = 38, MaltCategory = MaltCategory.Base }, "", "");

        internal static Fermentable ChocolateMalt => new("Chocolate Malt",
                new FermentableCharacteristics(60f, 350f, 0) { GravityPoint = 28, MaltCategory = MaltCategory.Roasted }, "", "");
    }
}