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

        [HttpGet("GetAll")]
        public List<FermentableDto> GetFermentables()
        {
            _logger.LogInformation($"Entering {nameof(GetFermentables)}");
            return _homebrewingDbService.GetFermentables();
        }

        [HttpGet("{fermentableId}")]
        public ActionResult<FermentableDto> GetFermentable([FromRoute] string fermentableId)
        {
            try
            {
                _logger.LogInformation($"Entering {nameof(GetFermentable)}");
                return _homebrewingDbService.GetFermentableDto(fermentableId);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid fermentable ID!");
                return new BadRequestObjectResult(new { message = "Invalid fermentable ID!" });
            }
        }
    }
}