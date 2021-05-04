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
            float colorComparison;
            List<IFermentableIngredient> grainBill;
            var baseGrainProportionDifferential = 0;
            var adjustedGristProportions = new Dictionary<CommonGrain, int>();
            do
            {
                grainBill = GetGrainBill(size, style, abv, adjustedGristProportions).ToList();

                var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, size);
                colorComparison = colorBasedOnGrainBill - colorSrm;
                baseGrainProportionDifferential = GetBaseGrainProportionDifferential(colorComparison, baseGrainProportionDifferential);
                adjustedGristProportions = GetAdjustedGristProportions(style.CommonGrains, grainBill, baseGrainProportionDifferential, size);
            }
            while (Math.Abs(colorComparison) >= 1);

            return new Recipe
            {
                Name = name,
                Size = size,
                BoilTime = RecipeDefaultSettings.BoilTime,
                Style = style,
                FermentableIngredients = grainBill
            };
        }

        private static IEnumerable<IFermentableIngredient> GetGrainBill(float size, IStyle style, float abv,
            Dictionary<CommonGrain, int> adjustedGristProportions)
        {
            if (style.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {style.Name} is missing common grains");
            
            foreach (var commonGrain in style.CommonGrains)
            {
                adjustedGristProportions.TryGetValue(commonGrain, out var proportionOfGrist);
                if (proportionOfGrist < 0) throw new InvalidOperationException("Can't have negative grain proportion");

                var poundsOfCommonGrain = FermentableUtility.GetPoundsRequired(
                    proportionOfGrist, size, abv, MashEfficiency, commonGrain.Fermentable.Characteristics.GravityPoint);
                yield return new FermentableIngredient { Amount = poundsOfCommonGrain, FermentableInfo = commonGrain.Fermentable };
            }
        }

        private static Dictionary<CommonGrain, int> GetAdjustedGristProportions(IReadOnlyCollection<CommonGrain> styleCommonGrains,
            IReadOnlyCollection<IFermentableIngredient> fermentableIngredients,
            int baseGrainProportionDifferential,
            float size)
        {
            var highestColorImpactGrain = styleCommonGrains.ToDictionary(g => g, g =>
            {
                var fermentableIngredient = fermentableIngredients.Single(f => f.FermentableInfo == g.Fermentable);
                return ColorUtility.GetMaltColorUnit(fermentableIngredient, size);
            })
                .OrderByDescending(x => x.Value)
                .First()
                .Key;
            var adjustedGristProportions = new Dictionary<CommonGrain, int>();
            foreach (var commonGrain in styleCommonGrains)
            {
                var adjustedProportion = commonGrain.Category == MaltCategory.Base ? commonGrain.ProportionOfGrist + baseGrainProportionDifferential :
                    commonGrain == highestColorImpactGrain ? commonGrain.ProportionOfGrist - baseGrainProportionDifferential :
                    commonGrain.ProportionOfGrist;
                adjustedGristProportions.Add(commonGrain, adjustedProportion);
            }
            return adjustedGristProportions;
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