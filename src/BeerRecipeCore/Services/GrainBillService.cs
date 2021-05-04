using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore.Services
{
    internal class GrainBillService
    {
        private const int MashEfficiency = 65;
        
        internal List<IFermentableIngredient> GetGrainBill(RecipeGenerationInfo recipeGenerationInfo)
        {
            float colorComparison;
            var grainBill = new List<IFermentableIngredient>();
            var baseGrainProportionDifferential = 0;
            do
            {
                var adjustedGristProportions = GetAdjustedGristProportions(recipeGenerationInfo, grainBill, baseGrainProportionDifferential);
                grainBill = GetGrainBillGivenAdjustedGristProportions(recipeGenerationInfo, adjustedGristProportions).ToList();

                colorComparison = GetColorComparisonOfGrainBillToDesiredColor(recipeGenerationInfo, grainBill);
                UpdateBaseGrainProportionDifferential(colorComparison, ref baseGrainProportionDifferential);
            } while (Math.Abs(colorComparison) >= 1);

            return grainBill.Where(g => g.Amount > 0).ToList();
        }

        private static float GetColorComparisonOfGrainBillToDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill)
        {
            var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, recipeGenerationInfo.Size);
            return colorBasedOnGrainBill - recipeGenerationInfo.ColorSrm;
        }

        private static IEnumerable<IFermentableIngredient> GetGrainBillGivenAdjustedGristProportions(RecipeGenerationInfo recipeGenerationInfo,
            List<AdjustedCommonGrain> adjustedGristProportions)
        {
            if (recipeGenerationInfo.Style.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {recipeGenerationInfo.Style.Name} is missing common grains");
            
            foreach (var (commonGrain, adjustedGristProportion) in adjustedGristProportions)
            {
                var poundsOfCommonGrain = FermentableUtility.GetPoundsRequired(adjustedGristProportion, recipeGenerationInfo.Size, recipeGenerationInfo.Abv,
                    MashEfficiency, commonGrain.GravityPoint);
                yield return new FermentableIngredient { Amount = poundsOfCommonGrain, FermentableInfo = commonGrain.Fermentable };
            }
        }

        private static List<AdjustedCommonGrain> GetAdjustedGristProportions(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> fermentableIngredients,
            int baseGrainProportionDifferential)
        {
            var highestColorImpactGrain = GetGrainWithHighestColorImpact(recipeGenerationInfo.Style.CommonGrains, fermentableIngredients, recipeGenerationInfo.Size);
            return recipeGenerationInfo.Style.CommonGrains.Select(cg =>
                new AdjustedCommonGrain(cg, GetAdjustedGrainProportion(baseGrainProportionDifferential, cg, highestColorImpactGrain))).ToList();
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
            if (fermentableIngredients.Count == 0) return null;
            return styleCommonGrains.ToDictionary(g => g, g =>
                {
                    var fermentableIngredient = fermentableIngredients.Single(f => f.FermentableInfo == g.Fermentable);
                    return ColorUtility.GetMaltColorUnit(fermentableIngredient, size);
                })
                .OrderByDescending(x => x.Value)
                .First()
                .Key;
        }

        private static void UpdateBaseGrainProportionDifferential(float colorComparison, ref int grainProportionDifferential)
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
        }

        private record AdjustedCommonGrain(CommonGrain CommonGrain, int AdjustedGristProportion);
    }
}