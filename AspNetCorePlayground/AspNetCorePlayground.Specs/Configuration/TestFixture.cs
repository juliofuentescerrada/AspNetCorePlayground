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

    public sealed class TestFixture : WebApplicationFactory<TestStartup>
    {
        private static readonly Checkpoint Checkpoint = new() { TablesToIgnore = new[] { "__EFMigrationsHistory" } };

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host
                .CreateDefaultBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(builder => builder
                    .UseStartup<TestStartup>()
                    .UseTestServer());
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder).MigrateDbContext();
        }
        
        public Task ResetDatabase()
        {
            return Checkpoint.Reset(Services.GetService<IConfiguration>().GetConnectionString("DefaultConnection"));
        }

        public async Task AddToDatabase<T>(params T[] items) where T : class
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