using System;
using System.Collections.Generic;
using System.Linq;
using BeerRecipeCore.Fermentables;

namespace BeerRecipeCore.Formulas
{
    public static class ColorUtility
    {
        public static double GetColorInSrm(IEnumerable<IFermentableIngredient> fermentables, float recipeVolumeInGallons)
        {
            // calculate MCU from each fermentable
            var totalMcu = fermentables.Sum(fermentable => GetMaltColorUnit(fermentable, recipeVolumeInGallons));

            // get SRM color using Morey equation
            return Math.Round(1.4922 * Math.Pow(totalMcu, 0.6859), 1);
        }

        internal static double GetMaltColorUnit(IFermentableIngredient fermentable, float recipeVolumeInGallons)
        {
            return fermentable.FermentableInfo.Characteristics.Color * fermentable.Amount / recipeVolumeInGallons;
        }
    }
}
