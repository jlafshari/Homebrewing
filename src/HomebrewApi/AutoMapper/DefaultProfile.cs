using AutoMapper;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Yeast;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using Fermentable = HomebrewApi.Models.Fermentable;
using FermentableCharacteristics = HomebrewApi.Models.FermentableCharacteristics;
using Yeast = HomebrewApi.Models.Yeast;
using YeastCharacteristics = HomebrewApi.Models.YeastCharacteristics;

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
                .ForMember(s => s.FermentableIngredients, opt => opt.Ignore())
                .ForMember(s => s.YeastIngredient, opt => opt.Ignore());

            CreateMap<Style, BeerRecipeCore.Styles.Style>()
                .ForMember(s => s.CommonGrains, opt => opt.Ignore())
                .ForMember(s => s.CommonYeast, opt => opt.Ignore());
            CreateMap<Style, StyleDto>();

            CreateMap<Fermentable, BeerRecipeCore.Fermentables.Fermentable>();
            CreateMap<FermentableCharacteristics, BeerRecipeCore.Fermentables.FermentableCharacteristics>();
            CreateMap<IFermentableIngredient, FermentableIngredient>();

            CreateMap<Yeast, BeerRecipeCore.Yeast.Yeast>();
            CreateMap<YeastCharacteristics, BeerRecipeCore.Yeast.YeastCharacteristics>();
            CreateMap<IYeastIngredient, YeastIngredient>();
            CreateMap<BeerRecipeCore.Yeast.Yeast, YeastIngredientDto>();
        }
    }
}