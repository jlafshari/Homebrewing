using System;
using System.Collections.Generic;
using HomebrewApi.Models;
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

        [HttpGet("GetAll")]
        public List<HopDto> GetHops()
        {
            _logger.LogInformation($"Entering {nameof(GetHops)}");
            return _homebrewingDbService.GetHops();
        }

        [HttpGet("{hopId}")]
        public ActionResult<HopDto> GetHop([FromRoute] string hopId)
        {
            try
            {
                _logger.LogInformation($"Entering {nameof(GetHop)}");
                return _homebrewingDbService.GetHopDto(hopId);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid hop ID!");
                return new BadRequestObjectResult(new { message = "Invalid hop ID!" });
            }
        }
    }
}