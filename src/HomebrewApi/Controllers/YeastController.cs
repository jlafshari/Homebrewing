using HomebrewApi.Models;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YeastController : ControllerBase
    {
        private readonly ILogger<YeastController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public YeastController(ILogger<YeastController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpPost]
        public void CreateYeast(Yeast yeast)
        {
            _logger.LogInformation("Creating yeast '{name}'", yeast.Name);
            _homebrewingDbService.CreateYeast(yeast);
        }
    }
}