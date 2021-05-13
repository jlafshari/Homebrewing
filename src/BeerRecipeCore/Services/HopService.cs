using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore.Services
{
    internal class HopService
    {
        internal List<IHopIngredient> GetHopIngredients(RecipeGenerationInfo recipeGenerationInfo,
            List<IFermentableIngredient> fermentableIngredients)
        {
            var originalGravity = AlcoholUtility.GetOriginalGravity(fermentableIngredients, recipeGenerationInfo.Size);
            var hopIngredients = new List<IHopIngredient>();
            foreach (var commonHop in recipeGenerationInfo.Style.CommonHops)
            {
                hopIngredients.Add(new HopIngredient
                {
                    HopInfo = commonHop.Hop,
                    FlavorType = commonHop.HopFlavorType,
                    Time = commonHop.BoilAdditionTime,
                    Amount = GetHopAmount(commonHop, recipeGenerationInfo, originalGravity)
                });
            }

            return hopIngredients;
        }

        private float GetHopAmount(CommonHop commonHop, RecipeGenerationInfo recipeGenerationInfo, float originalGravity)
        {
            var ibuContribution = (float) commonHop.IbuContributionPercentage / 100 * recipeGenerationInfo.Ibu;
            var hopUtilizationPercentage = BitternessUtility.GetHopUtilization(commonHop.BoilAdditionTime, originalGravity) * 100;
            return recipeGenerationInfo.Size * ibuContribution / (hopUtilizationPercentage * commonHop.Hop.Characteristics.AlphaAcid) / 0.7489f;
        }
    }
}