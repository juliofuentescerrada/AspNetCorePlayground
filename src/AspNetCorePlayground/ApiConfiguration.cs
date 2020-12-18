namespace AspNetCorePlayground
{
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WeatherForecast;

    public static class ApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeatherForecastServices();

            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<WeatherForecastDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddScoped<DbContext>(provider => provider.GetRequiredService<WeatherForecastDbContext>());
                
            
            services.AddControllers().AddWeatherForecastPart();

            return services;
        }

        public static void Configure(IApplicationBuilder app)
        {
            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}