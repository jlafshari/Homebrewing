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
        private ILogger<RecipeController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public RecipeController(ILogger<RecipeController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpGet("GetAll")]
        public List<RecipeDto> GetRecipes()
        {
            var userId = GetUserId();
            return _homebrewingDbService.GetRecipes(userId);
        }

        [HttpGet("{recipeId}")]
        public ActionResult<RecipeDto> GetRecipe([FromRoute] string recipeId)
        {
            try
            {
                return _homebrewingDbService.GetRecipe(recipeId);
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
            return _homebrewingDbService.GenerateRecipe(recipeGenerationInfoDto);
        }

        [HttpDelete("{recipeId}")]
        public ActionResult DeleteRecipe([FromRoute] string recipeId)
        {
            try
            {
                _homebrewingDbService.DeleteRecipe(recipeId);
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