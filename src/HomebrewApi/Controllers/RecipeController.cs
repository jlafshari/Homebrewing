using System;
using System.Collections.Generic;
using System.Linq;
using HomebrewApi.Models.Dtos;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public RecipeController(ILogger<RecipeController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpGet("GetAll")]
        public List<RecipeDto> GetRecipes()
        {
            _logger.LogInformation($"Entering {nameof(GetRecipes)}");
            var userId = GetUserId();
            return _homebrewingDbService.GetRecipes(userId);
        }

        [HttpGet("{recipeId}")]
        public ActionResult<RecipeDto> GetRecipe([FromRoute] string recipeId)
        {
            try
            {
                _logger.LogInformation($"Entering {nameof(GetRecipe)}");
                var userId = GetUserId();
                return _homebrewingDbService.GetRecipe(recipeId, userId);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid recipe ID!");
                return new BadRequestObjectResult(new { message = "Invalid recipe ID!" });
            }
        }

        [HttpPost("GenerateRecipe")]
        public RecipeDto GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto)
        {
            _logger.LogInformation($"Entering {nameof(GenerateRecipe)}");
            var userId = GetUserId();
            return _homebrewingDbService.GenerateRecipe(recipeGenerationInfoDto, userId);
        }

        [HttpDelete("{recipeId}")]
        public ActionResult DeleteRecipe([FromRoute] string recipeId)
        {
            try
            {
                _logger.LogInformation($"Entering {nameof(DeleteRecipe)}");
                var userId = GetUserId();
                _homebrewingDbService.DeleteRecipe(recipeId, userId);
                return new OkResult();
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid recipe ID!");
                return new BadRequestObjectResult(new { message = "Invalid recipe ID!" });
            }
        }

        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
        }
    }
}