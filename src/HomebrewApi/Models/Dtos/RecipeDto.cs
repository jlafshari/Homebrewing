using System.Collections.Generic;
using BeerRecipeCore.Recipes;

namespace HomebrewApi.Models.Dtos
{
    public record RecipeDto(string Id, float Size, string Name, RecipeProjectedOutcome ProjectedOutcome)
    {
        public string StyleName { get; set; }
        public List<FermentableIngredientDto> FermentableIngredients { get; } = new();
        public List<HopIngredientDto> HopIngredients { get; set; } = new();
        public YeastIngredientDto YeastIngredient { get; set; }
    }
}