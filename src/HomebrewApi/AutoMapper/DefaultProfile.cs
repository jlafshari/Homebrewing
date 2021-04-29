using AutoMapper;
using BeerRecipeCore;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;

namespace HomebrewApi.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<IRecipe, Recipe>()
                .ForMember(r => r.StyleId, opt => opt.MapFrom(src => src.Style.Name));
            CreateMap<RecipeGenerationInfoDto, RecipeProjectedOutcome>();
            CreateMap<Recipe, RecipeDto>();
            
            CreateMap<Style, StyleDto>();
        }
    }
}