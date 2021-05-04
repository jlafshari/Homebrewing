using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore.Services
{
    public class RecipeService
    {
        private const int MashEfficiency = 65;
        
        public IRecipe GenerateRecipe(float size, IStyle style, float abv, int colorSrm, string name)
        {
            var grainBill = GetGrainBill(size, style, abv, colorSrm);

            return new Recipe
            {
                Name = name,
                Size = size,
                BoilTime = RecipeDefaultSettings.BoilTime,
                Style = style,
                FermentableIngredients = grainBill
            };
        }

        private static List<IFermentableIngredient> GetGrainBill(float size, IStyle style, float abv, int colorSrm)
        {
            float colorComparison;
            List<IFermentableIngredient> grainBill;
            var baseGrainProportionDifferential = 0;
            var adjustedGristProportions = new Dictionary<CommonGrain, int>();
            do
            {
                grainBill = GetGrainBillGivenAdjustedGristProportions(size, style, abv, adjustedGristProportions).ToList();

                var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, size);
                colorComparison = colorBasedOnGrainBill - colorSrm;
                baseGrainProportionDifferential = GetBaseGrainProportionDifferential(colorComparison, baseGrainProportionDifferential);
                adjustedGristProportions = GetAdjustedGristProportions(style.CommonGrains, grainBill, baseGrainProportionDifferential, size);
            } while (Math.Abs(colorComparison) >= 1);

            return grainBill.Where(g => g.Amount > 0).ToList();
        }

        private static IEnumerable<IFermentableIngredient> GetGrainBillGivenAdjustedGristProportions(float size, IStyle style, float abv,
            Dictionary<CommonGrain, int> adjustedGristProportions)
        {
            if (style.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {style.Name} is missing common grains");
            
            foreach (var commonGrain in style.CommonGrains)
            {
                var proportionOfGrist = GetAdjustedGristProportionForGrain(adjustedGristProportions, commonGrain);
                var poundsOfCommonGrain = FermentableUtility.GetPoundsRequired(proportionOfGrist, size, abv, MashEfficiency, commonGrain.GravityPoint);
                yield return new FermentableIngredient { Amount = poundsOfCommonGrain, FermentableInfo = commonGrain.Fermentable };
            }
        }

        private static int GetAdjustedGristProportionForGrain(Dictionary<CommonGrain, int> adjustedGristProportions, CommonGrain commonGrain)
        {
            if (!adjustedGristProportions.TryGetValue(commonGrain, out var proportionOfGrist))
                proportionOfGrist = commonGrain.ProportionOfGrist;
            if (proportionOfGrist < 0) throw new InvalidOperationException("Can't have negative grain proportion");
            return proportionOfGrist;
        }

        private static Dictionary<CommonGrain, int> GetAdjustedGristProportions(IReadOnlyCollection<CommonGrain> styleCommonGrains,
            IReadOnlyCollection<IFermentableIngredient> fermentableIngredients,
            int baseGrainProportionDifferential,
            float size)
        {
            var highestColorImpactGrain = GetGrainWithHighestColorImpact(styleCommonGrains, fermentableIngredients, size);
            return styleCommonGrains.ToDictionary(cg => cg, cg => GetAdjustedGrainProportion(baseGrainProportionDifferential, cg, highestColorImpactGrain));
        }

        private static int GetAdjustedGrainProportion(int baseGrainProportionDifferential, CommonGrain commonGrain, CommonGrain highestColorImpactGrain)
        {
            return commonGrain.Category == MaltCategory.Base ? commonGrain.ProportionOfGrist + baseGrainProportionDifferential :
                commonGrain == highestColorImpactGrain ? commonGrain.ProportionOfGrist - baseGrainProportionDifferential :
                commonGrain.ProportionOfGrist;
        }

        private static CommonGrain GetGrainWithHighestColorImpact(IEnumerable<CommonGrain> styleCommonGrains,
            IReadOnlyCollection<IFermentableIngredient> fermentableIngredients, float size)
        {
            return styleCommonGrains.ToDictionary(g => g, g =>
                {
                    var fermentableIngredient = fermentableIngredients.Single(f => f.FermentableInfo == g.Fermentable);
                    return ColorUtility.GetMaltColorUnit(fermentableIngredient, size);
                })
                .OrderByDescending(x => x.Value)
                .First()
                .Key;
        }

        private static int GetBaseGrainProportionDifferential(float colorComparison, int grainProportionDifferential)
        {
            switch (colorComparison)
            {
                case > 0:
                    grainProportionDifferential++;
                    break;
                case < 0:
                    grainProportionDifferential--;
                    break;
            }

            return grainProportionDifferential;
        }
    }
}