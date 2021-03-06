using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Tests.Services
{
    internal static class RecipeServiceTestHelper
    {
        internal static Style GetStyle(string name, List<CommonGrain> commonGrains) =>
            GetStyle(name, commonGrains, new List<CommonHop>());

        internal static Style GetStyle(string name, List<CommonGrain> commonGrains, List<CommonHop> commonHops)
        {
            var style = new Style(name, new StyleCategory("", 1, StyleType.Ale),
                new StyleClassification("E", "test"), new List<StyleThreshold>())
            {
                CommonGrains = commonGrains,
                CommonHops = commonHops,
                CommonYeast = SafAleEnglishAleYeast
            };
            return style;
        }

        internal static CommonHop GetCommonHop(Hop hop, int boilAdditionTime, int ibuContributionPercentage) =>
            new()
            {
                BoilAdditionTime = boilAdditionTime,
                Hop = hop,
                IbuContributionPercentage = ibuContributionPercentage
            };

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

        internal static Hop Fuggles => new("Fuggles", new HopCharacteristics(4.5f, 4.0f, 10), "", "");

        internal static Hop Cascade => new("Cascade", new HopCharacteristics(5.5f, 4.3f, 10), "", "");

        internal static Yeast.Yeast SafAleEnglishAleYeast => new("Safale English Ale",
            new YeastCharacteristics(YeastType.Ale, Flocculation.Medium, YeastForm.Dry, 16, 22, 75), "", "DCL Labs", "");
    }
}