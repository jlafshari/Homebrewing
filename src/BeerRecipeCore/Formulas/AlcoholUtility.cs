using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerRecipeCore.Formulas
{
    public static class AlcoholUtility
    {
        /// <summary>
        /// Gets the percentage of alcohol by volume (ABV) produced between two specific gravity measurements.
        /// </summary>
        public static float GetAlcoholByVolume(float startingGravity, float finalGravity)
        {
            return (1.05f * (startingGravity - finalGravity)) / (finalGravity * 0.79f) * 100;
        }

        /// <summary>
        /// Gets the percentage of alcohol by weight (ABW) from a given ABV value.
        /// </summary>
        public static float GetAlcoholByWeight(float alcoholByVolume)
        {
            return alcoholByVolume * 0.79336f;
        }

        /// <summary>
        /// Gets the estimated original gravity using a default extraction efficiency.
        /// </summary>
        /// <param name="fermentableIngredients">The fermentable ingredients in the recipe.</param>
        /// <param name="recipeSize">The recipe size in gallons.</param>
        public static float GetOriginalGravity(IEnumerable<IFermentableIngredient> fermentableIngredients, float recipeSize)
        {
            return GetOriginalGravity(fermentableIngredients, recipeSize, c_defaultExtractionEfficiency);
        }

        /// <summary>
        /// Gets the estimated original gravity.
        /// </summary>
        /// <param name="fermentableIngredients">The fermentable ingredients in the recipe.</param>
        /// <param name="recipeSize">The recipe size in gallons.</param>
        /// <param name="extractionEfficiency">The expected extraction efficiency percentage.</param>
        public static float GetOriginalGravity(IEnumerable<IFermentableIngredient> fermentableIngredients, float recipeSize, float extractionEfficiency)
        {
            float gravityPoints = fermentableIngredients.Select(ingredient => ingredient.Amount * ingredient.FermentableInfo.Characteristics.GravityPoint).Sum();
            float pointsPerPound = (gravityPoints / recipeSize) * (extractionEfficiency / 100f);
            pointsPerPound = (float) Math.Round((double) pointsPerPound);
            return 1f + (pointsPerPound / 1000f);
        }

        const float c_defaultExtractionEfficiency = 60f;
    }
}
