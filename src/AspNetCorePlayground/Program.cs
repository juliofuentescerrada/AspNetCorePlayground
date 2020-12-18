namespace AspNetCorePlayground
{
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using System;
    using System.IO;

    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                var host = Host
                    .CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                    .Build();

                host.MigrateDbContext().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }

    public static class HostExtensions
    {
        public static IHost MigrateDbContext(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
            WeatherForecastDbContextInitializer.Migrate(context);
            return host;
        }
    }
}
