using InovolaTask.Application.Helper;

namespace InovolaTask.Application.Services.WeatherServices;

public interface IWeatherService
{
    GeneralResponse GetCityWather(string cityName);
    //Task<GeneralResponse> GetCityWather(string cityName);
}
