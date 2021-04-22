using System.Collections.Generic;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private ILogger<RecipeController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public RecipeController(ILogger<RecipeController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpGet("GetAll")]
        public List<Recipe> GetRecipes()
        {
            return _homebrewingDbService.GetRecipes();
        }

        [HttpPost("GenerateRecipe")]
        public RecipeDto GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto)
        {
            return _homebrewingDbService.GenerateRecipe(recipeGenerationInfoDto);
        }
    }
}