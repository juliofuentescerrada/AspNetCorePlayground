namespace AspNetCorePlayground
{
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build();

            host.MigrateDbContext().Run();
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
