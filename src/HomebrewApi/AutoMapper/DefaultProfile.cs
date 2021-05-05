using AutoMapper;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Recipes;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using Fermentable = HomebrewApi.Models.Fermentable;
using FermentableCharacteristics = HomebrewApi.Models.FermentableCharacteristics;

namespace HomebrewApi.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<IRecipe, Recipe>();
            CreateMap<RecipeGenerationInfoDto, RecipeProjectedOutcome>();
            CreateMap<RecipeGenerationInfoDto, RecipeGenerationInfo>();
            CreateMap<Recipe, RecipeDto>()
                .ForMember(s => s.FermentableIngredients, opt => opt.Ignore());

            CreateMap<Style, BeerRecipeCore.Styles.Style>()
                .ForMember(s => s.CommonGrains, opt => opt.Ignore());
            CreateMap<Style, StyleDto>();

            CreateMap<Fermentable, BeerRecipeCore.Fermentables.Fermentable>();
            CreateMap<FermentableCharacteristics, BeerRecipeCore.Fermentables.FermentableCharacteristics>();
            CreateMap<IFermentableIngredient, FermentableIngredient>();
        }
    }
}