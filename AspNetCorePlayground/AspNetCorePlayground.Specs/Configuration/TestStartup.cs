﻿namespace AspNetCorePlayground.Specs.Configuration
{
    using Acheve.AspNetCore.TestHost.Security;
    using Acheve.TestHost;
    using Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class TestStartup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            ApiConfiguration.ConfigureServices(services, Configuration).AddControllers().AddApplicationPart(typeof(WeatherForecastController).Assembly);
            services.AddAuthentication(options => options.DefaultScheme = TestServerDefaults.AuthenticationScheme).AddTestServer();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            ApiConfiguration.Configure(app);
        }
    }
}