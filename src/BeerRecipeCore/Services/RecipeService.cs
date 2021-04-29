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
            do
            {
                grainBill = GetGrainBill(size, style, abv, baseGrainProportionDifferential).ToList();

                var colorBasedOnGrainBill = (float) ColorUtility.GetColorInSrm(grainBill, size);
                colorComparison = colorBasedOnGrainBill - colorSrm;
                baseGrainProportionDifferential = GetBaseGrainProportionDifferential(colorComparison, baseGrainProportionDifferential);
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

        private static IEnumerable<IFermentableIngredient> GetGrainBill(float size, IStyle style, float abv, int baseGrainProportionDifferential)
        {
            if (style.CommonGrains.Count == 0) throw new InvalidOperationException($"Style {style.Name} is missing common grains");
            
            foreach (var commonGrain in style.CommonGrains)
            {
                var proportionOfGrist = commonGrain.Category == MaltCategory.Base ? commonGrain.ProportionOfGrist + baseGrainProportionDifferential :
                    commonGrain.Category == MaltCategory.Caramel ? commonGrain.ProportionOfGrist - baseGrainProportionDifferential :
                    commonGrain.ProportionOfGrist;

                var poundsOfCommonGrain = FermentableUtility.GetPoundsRequired(
                    proportionOfGrist, size, abv, MashEfficiency, commonGrain.Fermentable.Characteristics.GravityPoint);
                yield return new FermentableIngredient { Amount = poundsOfCommonGrain, FermentableInfo = commonGrain.Fermentable };
            }
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