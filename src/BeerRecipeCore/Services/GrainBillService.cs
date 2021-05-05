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
        private int _baseGrainProportionDifferential;
        
        internal List<IFermentableIngredient> GetGrainBill(RecipeGenerationInfo recipeGenerationInfo)
        {
            var grainBill = new List<IFermentableIngredient>();
            _baseGrainProportionDifferential = 0;
            do
            {
                grainBill = GetGrainBillGivenLastGenerationAttempt(recipeGenerationInfo, grainBill).ToList();
            }
            while (!DoesGrainBillColorMatchDesiredColor(recipeGenerationInfo, grainBill));

            return grainBill;
        }

        private IEnumerable<IFermentableIngredient> GetGrainBillGivenLastGenerationAttempt(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt)
        {
            if (recipeGenerationInfo.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {recipeGenerationInfo.StyleName} is missing common grains");
            
            var adjustedGristProportions = GetAdjustedGristProportions(recipeGenerationInfo, previousGrainBillAttempt);            
            foreach (var (commonGrain, adjustedGristProportion) in adjustedGristProportions)
            {
                var poundsOfGrain = FermentableUtility.GetPoundsRequired(adjustedGristProportion, recipeGenerationInfo.Size, recipeGenerationInfo.Abv,
                    MashEfficiency, commonGrain.GravityPoint);
                if (poundsOfGrain > 0)
                    yield return new FermentableIngredient { Amount = poundsOfGrain, FermentableInfo = commonGrain.Fermentable };
            }
        }

        private List<AdjustedCommonGrain> GetAdjustedGristProportions(RecipeGenerationInfo recipeGenerationInfo,
            IReadOnlyCollection<IFermentableIngredient> previousGrainBillAttempt)
        {
            var highestColorImpactGrain = GetGrainWithHighestColorImpact(recipeGenerationInfo.CommonGrains, previousGrainBillAttempt, recipeGenerationInfo.Size);
            return recipeGenerationInfo.CommonGrains.Select(cg =>
                new AdjustedCommonGrain(cg, GetAdjustedGrainProportion(cg, highestColorImpactGrain))).ToList();
        }

        private int GetAdjustedGrainProportion(CommonGrain commonGrain, CommonGrain highestColorImpactGrain)
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
                    var fermentableIngredient = fermentableIngredients.Single(f => f.FermentableInfo == g.Fermentable);
                    return ColorUtility.GetMaltColorUnit(fermentableIngredient, size);
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
                    _baseGrainProportionDifferential++;
                    break;
                case < 0:
                    _baseGrainProportionDifferential--;
                    break;
            }
        }

        private static float GetColorComparisonOfGrainBillToDesiredColor(RecipeGenerationInfo recipeGenerationInfo, IEnumerable<IFermentableIngredient> grainBill)
        {
            var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, recipeGenerationInfo.Size);
            return colorBasedOnGrainBill - recipeGenerationInfo.ColorSrm;
        }

        private record AdjustedCommonGrain(CommonGrain CommonGrain, int AdjustedGristProportion);
    }
}