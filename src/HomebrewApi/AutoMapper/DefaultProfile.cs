using AutoMapper;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Recipes;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using Fermentable = HomebrewApi.Models.Fermentable;
using FermentableCharacteristics = HomebrewApi.Models.FermentableCharacteristics;
using Hop = HomebrewApi.Models.Hop;
using HopCharacteristics = HomebrewApi.Models.HopCharacteristics;
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
                .ForMember(s => s.HopIngredients, opt => opt.Ignore())
                .ForMember(s => s.YeastIngredient, opt => opt.Ignore());

            CreateMap<Style, BeerRecipeCore.Styles.Style>()
                .ForMember(s => s.CommonGrains, opt => opt.Ignore())
                .ForMember(s => s.CommonHops, opt => opt.Ignore())
                .ForMember(s => s.CommonYeast, opt => opt.Ignore());
            CreateMap<Style, StyleDto>();

            CreateMap<Fermentable, BeerRecipeCore.Fermentables.Fermentable>();
            CreateMap<FermentableCharacteristics, BeerRecipeCore.Fermentables.FermentableCharacteristics>();
            CreateMap<IFermentableIngredient, FermentableIngredient>();

            CreateMap<Hop, BeerRecipeCore.Hops.Hop>();
            CreateMap<HopCharacteristics, BeerRecipeCore.Hops.HopCharacteristics>();
            CreateMap<IHopIngredient, HopIngredient>()
                .ForMember(s => s.BoilAdditionTime, opt => opt.MapFrom(src => src.Time));
            CreateMap<CommonHop, BeerRecipeCore.Styles.CommonHop>();

            CreateMap<Yeast, BeerRecipeCore.Yeast.Yeast>();
            CreateMap<YeastCharacteristics, BeerRecipeCore.Yeast.YeastCharacteristics>();
            CreateMap<BeerRecipeCore.Yeast.Yeast, YeastIngredientDto>();
        }
    }
}