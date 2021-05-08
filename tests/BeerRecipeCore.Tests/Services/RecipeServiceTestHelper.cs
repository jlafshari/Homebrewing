using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Tests.Services
{
    internal static class RecipeServiceTestHelper
    {
        internal static Fermentable VictoryMalt => new("Victory Malt",
                new FermentableCharacteristics(73f, 25f, 50f) { GravityPoint = 34, MaltCategory = MaltCategory.Caramel }, "", "");

        internal static Fermentable MarisOtter => new("Maris Otter",
                new FermentableCharacteristics(82.5f, 3f, 120f) { GravityPoint = 38, MaltCategory = MaltCategory.Base }, "", "");

        internal static Fermentable TwoRow => new("2 Row",
                new FermentableCharacteristics(79f, 2f, 140f) { GravityPoint = 36, MaltCategory = MaltCategory.Base }, "", "");

        internal static Fermentable ChocolateMalt => new("Chocolate Malt",
                new FermentableCharacteristics(60f, 350f, 0) { GravityPoint = 28, MaltCategory = MaltCategory.Roasted }, "", "");

        internal static Fermentable Crystal20LMalt => new("Crystal 20L Malt",
                new FermentableCharacteristics(75f, 20f, 0) { GravityPoint = 35, MaltCategory = MaltCategory.Caramel }, "", "");

        internal static Yeast.Yeast SafAleEnglishAleYeast => new("Safale English Ale",
                new YeastCharacteristics(YeastType.Ale, Flocculation.Medium, YeastForm.Dry, 16, 22, 75), "", "DCL Labs", "");
    }
}