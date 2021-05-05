using System.Collections.Generic;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore.Recipes
{
    public record RecipeGenerationInfo(float Size, float Abv, int ColorSrm, string Name)
    {
        public IStyle Style { get; set; }
        public List<CommonGrain> CommonGrains => Style.CommonGrains;
        public string StyleName => Style.Name;
    }
}