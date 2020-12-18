namespace AspNetCorePlayground.WeatherForecast
{
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(WeatherForecastController).Assembly);
            return services;
        }

        public static IMvcBuilder AddWeatherForecastPart(this IMvcBuilder services)
        {
            services.AddApplicationPart(typeof(WeatherForecastController).Assembly);
            return services;
        }
    }
}