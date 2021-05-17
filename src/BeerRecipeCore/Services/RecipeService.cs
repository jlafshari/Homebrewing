using BeerRecipeCore.Recipes;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Services
{
    public class RecipeService
    {
        private readonly HopService _hopService;

        public RecipeService()
        {
            _hopService = new HopService();
        }
        
        public IRecipe GenerateRecipe(RecipeGenerationInfo recipeGenerationInfo)
        {
            var grainBill = new GrainBillService().GetGrainBill(recipeGenerationInfo);
            var hopIngredients = _hopService.GetHopIngredients(recipeGenerationInfo, grainBill);

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
    }
}