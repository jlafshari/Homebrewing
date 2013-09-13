using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerRecipeCore.Formulas
{
    public static class ColorUtility
    {
        public static float GetColorInSrm(IEnumerable<FermentableIngredient> fermentables, float recipeVolumeInGallons)
        {
            // calculate MCU from each fermentable
            float totalMcu = 0;
            foreach (FermentableIngredient fermentable in fermentables)
            {
                float fermentableMcu = GetMaltColorUnit(fermentable, recipeVolumeInGallons);
                totalMcu += fermentableMcu;
            }
            
            // get SRM color using Morey equation
            return 1.4922f * (float) Math.Pow(totalMcu, 0.6859);
        }

        private static float GetMaltColorUnit(FermentableIngredient fermentable, float recipeVolumeInGallons)
        {
            return fermentable.FermentableInfo.Characteristics.Color * fermentable.Amount / recipeVolumeInGallons;
        }
    }
}
