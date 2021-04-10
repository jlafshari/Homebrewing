using System.Collections.Generic;
using BeerRecipeCore.Styles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private ILogger<RecipeController> _logger;

        public RecipeController(ILogger<RecipeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<IStyle> GetBeerStyles()
        {
            return null;
        }
    }
}