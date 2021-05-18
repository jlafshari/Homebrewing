using System.ComponentModel.DataAnnotations;
using Constants = BeerRecipeCore.Recipes.RecipeGenerationConstants;

namespace HomebrewApi.Models.Dtos
{
    public record RecipeGenerationInfoDto(
        [Required][Range(Constants.MinimumSize, Constants.MaximumSize)]float Size,
        [Required]string StyleId,
        [Required][Range(Constants.MinimumAbv, Constants.MaximumAbv)]float Abv,
        [Required][Range(Constants.MinimumColor, Constants.MaximumColor)]int ColorSrm,
        [Required][Range(Constants.MinimumIbu, Constants.MaximumIbu)]int Ibu,
        [Required]string Name);
}