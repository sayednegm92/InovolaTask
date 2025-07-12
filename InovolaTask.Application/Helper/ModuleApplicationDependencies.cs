using InovolaTask.Application.BaseRepository;
using InovolaTask.Application.Services.AuthServices;
using InovolaTask.Application.Services.WeatherServices;
using Microsoft.Extensions.DependencyInjection;
namespace InovolaTask.Application.Helper;

public static class ModuleApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryApp<>), typeof(RepositoryApp<>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IResponseHandler, ResponseHandler>();
        services.AddScoped<IWeatherService, WeatherService>();
        return services;
    }

}
