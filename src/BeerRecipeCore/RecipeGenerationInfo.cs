using BeerRecipeCore.Styles;

namespace BeerRecipeCore
{
    public record RecipeGenerationInfo(float Size, float Abv, int ColorSrm, string Name)
    {
        public IStyle Style { get; set; }
    }
}