using HomebrewApi.Models;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomebrewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HopsController : ControllerBase
    {
        private readonly ILogger<HopsController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public HopsController(ILogger<HopsController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpPost]
        public void CreateHops(Hops hops)
        {
            _logger.LogInformation("Creating hops: '{name}'", hops.Name);
            _homebrewingDbService.CreateHops(hops);
        }
    }
}