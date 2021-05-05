using System;

namespace BeerRecipeCore.Formulas
{
    public static class FermentableUtility
    {
        public static float GetPoundsRequired(float proportionOfGrist, float recipeSize, float abv,
            int mashEfficiency, int optimumYield)
        {
            var og = GetOriginalGravity(abv);
            var targetGravityUnit = AlcoholUtility.GetGravityUnit(og);
            var poundsRequired = proportionOfGrist * recipeSize * targetGravityUnit / (mashEfficiency * optimumYield);
            return (float) Math.Round(poundsRequired, 1);
        }

        public static float GetOriginalGravity(float abv)
        {
            return (float) Math.Round((abv / 131.6f) + 1.01f, 3);
        }
    }
}