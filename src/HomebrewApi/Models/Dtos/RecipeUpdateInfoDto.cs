using System.Collections.Generic;

namespace HomebrewApi.Models.Dtos
{
    public record RecipeUpdateInfoDto(string Name)
    {
        public List<FermentableIngredientDto> FermentableIngredients { get; init; } = new();
        public List<HopIngredientDto> HopIngredients { get; init; } = new();
    }
}