using InovolaTask.Application.Dto;
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
        [HttpGet("FindCity")]
        public async Task<IActionResult> FindCity(string cityName)
        {
            var result = await _service.FindCity(cityName);
            return Ok(result);
        }

        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _service.GetAllCities();
            return Ok(result);
        }

        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity(CityDto city)
        {
            var result = await _service.AddCityAsync(city);
            return Ok(result);
        }
        [HttpPut("EditCity")]
        public async Task<IActionResult> EditCity(EditCityDto city)
        {
            var result = await _service.EditCityAsync(city);
            return Ok(result);
        }
        [HttpDelete("DeleteCity")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            var result = await _service.DeleteCityAsync(cityId);
            return Ok(result);
        }
        #endregion

    }
}
