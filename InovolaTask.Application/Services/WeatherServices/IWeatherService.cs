using InovolaTask.Application.Dto;
using InovolaTask.Application.Helper;

namespace InovolaTask.Application.Services.WeatherServices;

public interface IWeatherService
{
    Task<GeneralResponse> GetCityWather(string cityName);
    Task<GeneralResponse> GetAllCities();
    Task<GeneralResponse> FindCity(string cityName);
    Task<GeneralResponse> AddCityAsync(CityDto city);
    Task<GeneralResponse> EditCityAsync(EditCityDto city);
    Task<GeneralResponse> DeleteCityAsync(int cityId);
}
