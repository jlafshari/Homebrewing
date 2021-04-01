﻿using System;
using System.Collections.Generic;
using BeerRecipeCore.Hops;

namespace BeerRecipeCore.Formulas
{
    public static class BitternessUtility
    {
        /// <summary>
        /// Gets the bitterness of a recipe in IBU units.
        /// </summary>
        /// <param name="hops">The hops used in the recipe.</param>
        /// <param name="recipeVolumeInGallons">The size of the recipe in gallons.</param>
        /// <param name="boilGravity">The specific gravity of the recipe before boiling.</param>
        public static int GetBitterness(IEnumerable<IHopsIngredient> hops, float recipeVolumeInGallons, float boilGravity)
        {
            float totalBitterness = 0;

            foreach (IHopsIngredient hopIngredient in hops)
            {
                float hopUtilization = GetHopUtilization(hopIngredient.Time, boilGravity);

                float alphaAcid = hopIngredient.HopsInfo.Characteristics.AlphaAcid;
                totalBitterness += hopUtilization * alphaAcid * hopIngredient.Amount * c_ibuEnglishUnitsConstant / recipeVolumeInGallons;
            }

            return (int) Math.Round((double) totalBitterness);
        }

        private static float GetHopUtilization(int boilTimeInMinutes, float boilGravity)
        {
            float bignessFactor = 1.65f * (float) Math.Pow(0.000125, boilGravity - 1);
            float boilTimeFactor = (1 - (float) Math.Pow(Math.E, -0.04 * boilTimeInMinutes)) / 4.15f;
            return bignessFactor * boilTimeFactor;
        }

        const float c_ibuEnglishUnitsConstant = 74.89f;
    }
}
