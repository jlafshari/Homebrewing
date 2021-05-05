using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore.Services
{
    internal class GrainBillService
    {
        private const int MashEfficiency = 65;
        private float _baseGrainProportionDifferential;
        private const float BaseGrainProportionDifferentialIncrement = 0.5f;
        
        internal List<IFermentableIngredient> GetGrainBill(RecipeGenerationInfo recipeGenerationInfo)
        {
            var grainBill = new List<IFermentableIngredient>();
            _baseGrainProportionDifferential = 0;
            do
            {
                grainBill = GetGrainBillGivenLastGenerationAttempt(recipeGenerationInfo, grainBill).ToList();
            }
            while (!DoesGrainBillColorMatchDesiredColor(recipeGenerationInfo, grainBill));

            return grainBill.Where(g => g.Amount > 0).ToList();
        }

        private IEnumerable<IFermentableIngredient> GetGrainBillGivenLastGenerationAttempt(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt)
        {
            if (recipeGenerationInfo.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {recipeGenerationInfo.StyleName} is missing common grains");
            
            foreach (var adjustedCommonGrain in GetAdjustedGristProportions(recipeGenerationInfo, previousGrainBillAttempt))
            {
                var poundsOfGrain = FermentableUtility.GetPoundsRequired(adjustedCommonGrain.AdjustedGristProportion, recipeGenerationInfo.Size, recipeGenerationInfo.Abv,
                    MashEfficiency, adjustedCommonGrain.CommonGrain.GravityPoint);
                yield return new FermentableIngredient { Amount = poundsOfGrain, FermentableInfo = adjustedCommonGrain.CommonGrain.Fermentable };
            }
        }

        private List<AdjustedCommonGrain> GetAdjustedGristProportions(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt)
        {
            var highestColorImpactGrain = GetGrainWithHighestColorImpact(recipeGenerationInfo.CommonGrains, previousGrainBillAttempt, recipeGenerationInfo.Size);
            var adjustedGristProportions = GetCurrentCommonGrains(recipeGenerationInfo, previousGrainBillAttempt).Select(cg =>
                new AdjustedCommonGrain(cg, GetAdjustedGrainProportion(cg, highestColorImpactGrain))).ToList();
            RebalanceGristProportionsIfNecessary(adjustedGristProportions);
            return adjustedGristProportions;
        }

        private static void RebalanceGristProportionsIfNecessary(List<AdjustedCommonGrain> adjustedGristProportions)
        {
            var proportionSum = adjustedGristProportions.Sum(a => a.AdjustedGristProportion);
            if (!(proportionSum < 100)) return;
            
            var baseMaltGrain = adjustedGristProportions.Single(a => a.CommonGrain.Category == MaltCategory.Base);
            baseMaltGrain.AdjustedGristProportion += 100 - proportionSum;
        }

        private static IEnumerable<CommonGrain> GetCurrentCommonGrains(RecipeGenerationInfo recipeGenerationInfo, IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt)
        {
            return previousGrainBillAttempt.Count == 0 ?
                recipeGenerationInfo.CommonGrains :
                recipeGenerationInfo.CommonGrains.Where(cg => previousGrainBillAttempt.Any(pg => pg.FermentableInfo == cg.Fermentable && pg.Amount > 0));
        }

        private float GetAdjustedGrainProportion(CommonGrain commonGrain, CommonGrain highestColorImpactGrain)
        {
            return commonGrain.Category == MaltCategory.Base ? commonGrain.ProportionOfGrist + _baseGrainProportionDifferential :
                commonGrain == highestColorImpactGrain ? commonGrain.ProportionOfGrist - _baseGrainProportionDifferential :
                commonGrain.ProportionOfGrist;
        }

        private static CommonGrain GetGrainWithHighestColorImpact(IEnumerable<CommonGrain> styleCommonGrains,
            IReadOnlyCollection<IFermentableIngredient> fermentableIngredients, float size)
        {
            if (fermentableIngredients.Count == 0) return null;
            return styleCommonGrains.ToDictionary(g => g, g =>
                {
                    var fermentableIngredient = fermentableIngredients.SingleOrDefault(f => f.FermentableInfo == g.Fermentable);
                    return fermentableIngredient != null ? ColorUtility.GetMaltColorUnit(fermentableIngredient, size) : 0;
                })
                .OrderByDescending(x => x.Value)
                .First()
                .Key;
        }

        private bool DoesGrainBillColorMatchDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill)
        {
            var colorComparison = GetColorComparisonOfGrainBillToDesiredColor(recipeGenerationInfo, grainBill);
            UpdateBaseGrainProportionDifferential(colorComparison);

            return Math.Abs(colorComparison) < 1;
        }

        private void UpdateBaseGrainProportionDifferential(float colorComparison)
        {
            switch (colorComparison)
            {
                case > 0:
                    _baseGrainProportionDifferential += BaseGrainProportionDifferentialIncrement;
                    break;
                case < 0:
                    _baseGrainProportionDifferential -= BaseGrainProportionDifferentialIncrement;
                    break;
            }
        }

        private static float GetColorComparisonOfGrainBillToDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill)
        {
            var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, recipeGenerationInfo.Size);
            return colorBasedOnGrainBill - recipeGenerationInfo.ColorSrm;
        }

        private record AdjustedCommonGrain
        {
            public AdjustedCommonGrain(CommonGrain commonGrain, float adjustedGristProportion)
            {
                CommonGrain = commonGrain;
                AdjustedGristProportion = adjustedGristProportion;
            }
            
            public CommonGrain CommonGrain { get; }
            public float AdjustedGristProportion { get; set; }
        }
    }
}