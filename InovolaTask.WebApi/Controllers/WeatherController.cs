using InovolaTask.Application.Services.WeatherServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InovolaTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        #region Fields
        private readonly IWeatherService _service;
        #endregion
        #region Constractor
        public WeatherController(IWeatherService service)
        {
            _service = service;
        }
        #endregion
        #region Actions
        [HttpGet("GetCity")]
        public IActionResult GetCityWather([FromQuery] string cityName)
        {
            var result = _service.GetCityWather(cityName);
            return Ok(result);
        }
        #endregion

    }
}
