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
    public class StyleController : ControllerBase
    {
        private ILogger<StyleController> _logger;
        private readonly HomebrewingDbService _homebrewingDbService;

        public StyleController(ILogger<StyleController> logger, HomebrewingDbService homebrewingDbService)
        {
            _logger = logger;
            _homebrewingDbService = homebrewingDbService;
        }

        [HttpGet("GetAll")]
        public List<StyleDto> GetBeerStyles()
        {
            return _homebrewingDbService.GetBeerStyles();
        }

        [HttpPost]
        public void CreateBeerStyle(Style beerStyle)
        {
            _homebrewingDbService.CreateBeerStyle(beerStyle);
        }
    }
}