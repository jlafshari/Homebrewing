using HomebrewApi.Models;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HopController : ControllerBase
    {
        private readonly ILogger<HopController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public HopController(ILogger<HopController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpPost]
        public void CreateHop(Hop hop)
        {
            _logger.LogInformation("Creating hop: '{name}'", hop.Name);
            _homebrewingDbService.CreateHop(hop);
        }
    }
}