using AutoMapper;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;

namespace HomebrewApi.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<RecipeGenerationInfoDto, RecipeProjectedOutcome>();
            CreateMap<RecipeGenerationInfoDto, Recipe>()
                .AfterMap((src, dest, context) => dest.ProjectedOutcome = context.Mapper.Map<RecipeGenerationInfoDto, RecipeProjectedOutcome>(src));
            CreateMap<Recipe, RecipeDto>();
        }
    }
}