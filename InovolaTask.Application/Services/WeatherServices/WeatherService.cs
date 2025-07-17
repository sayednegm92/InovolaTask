using InovolaTask.Application.BaseRepository;
using InovolaTask.Application.Dto;
using InovolaTask.Application.Helper;
using InovolaTask.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InovolaTask.Application.Services.WeatherServices;

public class WeatherService : IWeatherService
{
    private readonly IResponseHandler _responseHandler;
    private readonly IRepositoryApp<City> _cityRepo;
    private ILogger<WeatherService> _logger;
    private readonly IMemoryCache _memoryCache;
    private const string CityCacheKey = "city_list";
    public WeatherService(IRepositoryApp<City> cityRepo, IResponseHandler responseHandler, IMemoryCache memoryCache, ILogger<WeatherService> logger)
    {
        _cityRepo = cityRepo;
        _responseHandler = responseHandler;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    public GeneralResponse GetCityWather(string cityName)
    {

        // Check if the cities are cached
        if (_memoryCache.TryGetValue(CityCacheKey, out IEnumerable<City>? cachedCities))
        {
            _logger.LogInformation("Cities retrieved from cache.");
        }
        else
        {
            _logger.LogInformation("Cities not found in cache, fetching from Database.");

            cachedCities = _cityRepo.Seach(c => c.Name, cityName);
            if (cachedCities.Count() == 0)
                return _responseHandler.ShowMessage("City not found");

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)) // Cache will expire after 60 minutes of inactivity
               .SetAbsoluteExpiration(TimeSpan.FromHours(1)) // Cache will expire after 1 hour regardless of activity
               .SetPriority(CacheItemPriority.Normal); // Keep this item in memory longer
            _memoryCache.Set(CityCacheKey, cachedCities, cacheOptions);
        }

        return _responseHandler.Success(cachedCities);
    }
    public async Task<GeneralResponse> FindCity(string cityName)
    {
        // Check if the cities are cached
        if (_memoryCache.TryGetValue(CityCacheKey, out City? cachedCity))
        {
            _logger.LogInformation("Cities retrieved from cache.");
        }
        else
        {
            _logger.LogInformation("Cities not found in cache, fetching from Database.");

            cachedCity = await _cityRepo.FirstOrDefaultAsync(c => c.Name.Contains(cityName));
            if (cachedCity == null)
                return _responseHandler.ShowMessage("No cities found");

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
               .SetAbsoluteExpiration(TimeSpan.FromHours(1))
               .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(CityCacheKey, cachedCity, cacheOptions);
        }
        return _responseHandler.Success(cachedCity);
    }
    public async Task<GeneralResponse> GetAllCities()
    {
        // Check if the cities are cached
        if (_memoryCache.TryGetValue(CityCacheKey, out IEnumerable<City>? cachedCities))
        {
            _logger.LogInformation("Cities retrieved from cache.");
        }
        else
        {
            _logger.LogInformation("Cities not found in cache, fetching from Database.");

            cachedCities = await _cityRepo.GetAllAsync();
            if (cachedCities.Count() == 0)
                return _responseHandler.ShowMessage("No cities found");

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)) // Cache will expire after 60 minutes of inactivity
               .SetAbsoluteExpiration(TimeSpan.FromHours(1)) // Cache will expire after 1 hour regardless of activity
               .SetPriority(CacheItemPriority.Normal); // Keep this item in memory longer
            _memoryCache.Set(CityCacheKey, cachedCities, cacheOptions);
        }

        return _responseHandler.Success(cachedCities);
    }

    public async Task<GeneralResponse> AddCityAsync(CityDto city)
    {
        if (city == null)
        {
            return _responseHandler.ErrorMessage("City cannot be null");
        }
        var existingCity = await _cityRepo.FirstOrDefaultAsync(c => c.Name == city.Name);
        if (existingCity != null)
        {
            return _responseHandler.ShowMessage("City already exists");
        }
        var newCity = new City
        {
            Name = city.Name,
            TemperatureC = city.TemperatureC,
            Country = city.Country,
        };
        await _cityRepo.AddAsync(newCity);

        _memoryCache.Remove(CityCacheKey);

        return _responseHandler.Success(city, null, "City added successfully");

    }
    public async Task<GeneralResponse> EditCityAsync(EditCityDto city)
    {
        if (city == null)
        {
            return _responseHandler.ErrorMessage("City cannot be null");
        }
        var existingCity = await _cityRepo.FirstOrDefaultAsync(c => c.Id == city.Id);
        if (existingCity == null)
        {
            return _responseHandler.ShowMessage("City not found");
        }
        existingCity.Name = city.Name;
        existingCity.TemperatureC = city.TemperatureC;
        existingCity.Country = city.Country;

        await _cityRepo.UpdateAsync(existingCity);

        _memoryCache.Remove(CityCacheKey);

        return _responseHandler.Success(city, null, "City updated successfully");

    }
    public async Task<GeneralResponse> DeleteCityAsync(int cityId)
    {
        var existingCity = await _cityRepo.GetByIdAsync(cityId);
        if (existingCity == null)
        {
            return _responseHandler.ShowMessage("City not found");
        }

        await _cityRepo.DeleteAsync(existingCity);

        _memoryCache.Remove(CityCacheKey);

        return _responseHandler.Success(null, null, "City deleted successfully");


    }
}
