namespace BeerRecipeCore.Services
{
    public class RecipeService
    {
        private readonly GrainBillService _grainBillService;

        public RecipeService()
        {
            _grainBillService = new GrainBillService();
        }
        
        public IRecipe GenerateRecipe(RecipeGenerationInfo recipeGenerationInfo)
        {
            var grainBill = _grainBillService.GetGrainBill(recipeGenerationInfo);

            return new Recipe
            {
                Name = recipeGenerationInfo.Name,
                Size = recipeGenerationInfo.Size,
                BoilTime = RecipeDefaultSettings.BoilTime,
                Style = recipeGenerationInfo.Style,
                FermentableIngredients = grainBill
            };
        }
    }
}