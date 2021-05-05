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
            var grainBill = new List<IFermentableIngredient>();
            var baseGrainProportionDifferential = 0;
            do
            {
                grainBill = GetGrainBillGivenLastGenerationAttempt(recipeGenerationInfo, grainBill, baseGrainProportionDifferential).ToList();
            }
            while (!DoesGrainBillColorMatchDesiredColor(recipeGenerationInfo, grainBill, ref baseGrainProportionDifferential));

            return grainBill;
        }

        private static IEnumerable<IFermentableIngredient> GetGrainBillGivenLastGenerationAttempt(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt,
            int baseGrainProportionDifferential)
        {
            if (recipeGenerationInfo.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {recipeGenerationInfo.StyleName} is missing common grains");
            
            var adjustedGristProportions = GetAdjustedGristProportions(recipeGenerationInfo, previousGrainBillAttempt, baseGrainProportionDifferential);            
            foreach (var (commonGrain, adjustedGristProportion) in adjustedGristProportions)
            {
                var poundsOfGrain = FermentableUtility.GetPoundsRequired(adjustedGristProportion, recipeGenerationInfo.Size, recipeGenerationInfo.Abv,
                    MashEfficiency, commonGrain.GravityPoint);
                if (poundsOfGrain > 0)
                    yield return new FermentableIngredient { Amount = poundsOfGrain, FermentableInfo = commonGrain.Fermentable };
            }
        }

        private static List<AdjustedCommonGrain> GetAdjustedGristProportions(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt,
            int baseGrainProportionDifferential)
        {
            var highestColorImpactGrain = GetGrainWithHighestColorImpact(recipeGenerationInfo.CommonGrains, previousGrainBillAttempt, recipeGenerationInfo.Size);
            return recipeGenerationInfo.CommonGrains.Select(cg =>
                new AdjustedCommonGrain(cg, GetAdjustedGrainProportion(cg, baseGrainProportionDifferential, highestColorImpactGrain))).ToList();
        }

        private static int GetAdjustedGrainProportion(CommonGrain commonGrain, int baseGrainProportionDifferential, CommonGrain highestColorImpactGrain)
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

        private static bool DoesGrainBillColorMatchDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill, ref int grainProportionDifferential)
        {
            var colorComparison = GetColorComparisonOfGrainBillToDesiredColor(recipeGenerationInfo, grainBill);
            switch (colorComparison)
            {
                case > 0:
                    grainProportionDifferential++;
                    break;
                case < 0:
                    grainProportionDifferential--;
                    break;
            }

            return Math.Abs(colorComparison) < 1;
        }

        private static float GetColorComparisonOfGrainBillToDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill)
        {
            var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, recipeGenerationInfo.Size);
            return colorBasedOnGrainBill - recipeGenerationInfo.ColorSrm;
        }

        private record AdjustedCommonGrain(CommonGrain CommonGrain, int AdjustedGristProportion);
    }
}