using System;
using System.Collections.Generic;

namespace BeerRecipeCore.Formulas
{
    public static class ColorUtility
    {
        public static float GetColorInSrm(IEnumerable<IFermentableIngredient> fermentables, float recipeVolumeInGallons)
        {
            // calculate MCU from each fermentable
            float totalMcu = 0;
            foreach (IFermentableIngredient fermentable in fermentables)
            {
                float fermentableMcu = GetMaltColorUnit(fermentable, recipeVolumeInGallons);
                totalMcu += fermentableMcu;
            }
            
            // get SRM color using Morey equation
            return 1.4922f * (float) Math.Pow(totalMcu, 0.6859);
        }

        private static float GetMaltColorUnit(IFermentableIngredient fermentable, float recipeVolumeInGallons)
        {
            return fermentable.FermentableInfo.Characteristics.Color * fermentable.Amount / recipeVolumeInGallons;
        }
    }
}
