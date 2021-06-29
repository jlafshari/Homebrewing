using System;
using System.Collections.Generic;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Services
{
    public class RecipeService
    {
        public IRecipe GenerateRecipe(RecipeGenerationInfo recipeGenerationInfo)
        {
            var grainBill = new GrainBillService().GetGrainBill(recipeGenerationInfo);
            var hopIngredients = HopService.GetHopIngredients(recipeGenerationInfo, grainBill);

            return new Recipe
            {
                Name = recipeGenerationInfo.Name,
                Size = recipeGenerationInfo.Size,
                BoilTime = RecipeDefaultSettings.BoilTime,
                Style = recipeGenerationInfo.Style,
                FermentableIngredients = grainBill,
                HopIngredients = hopIngredients,
                YeastIngredient = new YeastIngredient(0, 0, recipeGenerationInfo.Style.CommonYeast)
            };
        }

        public RecipeProjectedOutcome GetRecipeProjectedOutcome(float recipeSize, List<IFermentableIngredient> fermentableIngredients,
            BeerRecipeCore.Yeast.Yeast yeast, int ibu)
        {
            var og = AlcoholUtility.GetOriginalGravity(fermentableIngredients, recipeSize);
            var fg = AlcoholUtility.GetFinalGravity(og, yeast.Characteristics.Attenuation);
            
            return new RecipeProjectedOutcome
            {
                Abv = AlcoholUtility.GetAlcoholByVolume(og, fg),
                ColorSrm = (int) Math.Round(ColorUtility.GetColorInSrm(fermentableIngredients, recipeSize), 0, MidpointRounding.ToEven),
                Ibu = ibu //TODO: update IBU when hops are updated
            };
        }
    }
}