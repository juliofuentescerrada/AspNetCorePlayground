namespace AspNetCorePlayground.Specs.Configuration
{
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Respawn;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public sealed partial class TestFixture : WebApplicationFactory<TestStartup>
    {
        private static readonly Checkpoint Checkpoint = new()
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        protected override IHostBuilder CreateHostBuilder() => Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder => builder.UseStartup<TestStartup>().UseTestServer());

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder.UseContentRoot(Directory.GetCurrentDirectory()));
            return MigrateDbContext(host);
        }

        public static IHost MigrateDbContext(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
            WeatherForecastDbContextInitializer.Migrate(context);
            return host;
        }

        public async Task ResetDatabase()
        {
            var connectionString = Services.GetService<IConfiguration>().GetConnectionString("DefaultConnection");
            await Checkpoint.Reset(connectionString);
        }

        public async Task AddDatabaseItems<T>(params T[] items) where T : class
        {
            using var scope = Services.CreateScope();
            await using var db = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
            await db.Set<T>().AddRangeAsync(items);
            await db.SaveChangesAsync();
        }
    }
    
    [CollectionDefinition(nameof(TestFixture))]
    public sealed class TestFixtureCollection : ICollectionFixture<TestFixture> { }
}