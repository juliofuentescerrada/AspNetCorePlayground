namespace AspNetCorePlayground.Data
{
    using Microsoft.EntityFrameworkCore;
    using Model;

    public sealed class WeatherForecastDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        
        public WeatherForecastDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}