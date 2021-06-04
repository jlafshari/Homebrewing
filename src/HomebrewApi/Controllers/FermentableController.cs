using HomebrewApi.Models;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FermentableController : ControllerBase
    {
        private readonly ILogger<FermentableController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public FermentableController(ILogger<FermentableController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpPost]
        public void CreateFermentable(Fermentable fermentable)
        {
            _logger.LogInformation("Creating fermentable '{name}'", fermentable.Name);
            _homebrewingDbService.CreateFermentable(fermentable);
        }
    }
}