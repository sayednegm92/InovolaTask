using InovolaTask.Application.BaseRepository;
using InovolaTask.Application.Helper;
using InovolaTask.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace InovolaTask.Application.Services.WeatherServices;

public class WeatherService : IWeatherService
{
    private readonly IResponseHandler _responseHandler;
    private readonly IRepositoryApp<City> _cityRepo;
    private readonly IMemoryCache _memoryCache;
    private const string CityCacheKey = "city_list";
    public WeatherService(IRepositoryApp<City> cityRepo, IResponseHandler responseHandler, IMemoryCache memoryCache)
    {
        _cityRepo = cityRepo;
        _responseHandler = responseHandler;
        _memoryCache = memoryCache;
    }
    public GeneralResponse GetCityWather(string cityName)
    {

        // Check if the cities are cached
        if (!_memoryCache.TryGetValue(CityCacheKey, out IEnumerable<City>? cachedCities))
        {
            // If not cached, retrieve from the repository
            cachedCities = _cityRepo.Seach(c => c.Name, cityName);
            if (cachedCities.Count() == 0)
                return _responseHandler.ShowMessage("City not found");
            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            // Store in cache
            _memoryCache.Set(CityCacheKey, cachedCities, cacheOptions);
        }

        return _responseHandler.Success(cachedCities);
    }
}
