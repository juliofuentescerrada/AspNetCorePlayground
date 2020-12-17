namespace AspNetCorePlayground.Data
{
    using Microsoft.EntityFrameworkCore;
    using Model;
    using System;
    using System.Linq;

    public static class WeatherForecastDbContextInitializer
    {
        public static void Migrate(WeatherForecastDbContext dbContext)
        {
            if (!dbContext.Database.GetMigrations().Any() || dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            if (!dbContext.WeatherForecasts.Any())
            {
                Seed(dbContext);
            }
        }

        private static void Seed(WeatherForecastDbContext dbContext)
        {
            dbContext.WeatherForecasts.Add(new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 25,
                Summary = "Summary"
            });
            
            dbContext.SaveChanges();
        }
    }
}