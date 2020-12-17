namespace AspNetCorePlayground.Specs.Specs
{
    using Configuration;
    using Controllers;
    using Data.Model;
    using Extensions;
    using FluentAssertions;
    using Given;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(nameof(TestFixture))]
    public sealed class weather_forecast_controller_should : IAsyncLifetime
    {
        private readonly TestFixture _fixture;
        public weather_forecast_controller_should(TestFixture fixture) => _fixture = fixture;
        public Task InitializeAsync() => _fixture.ResetDatabase();
        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task get()
        {
            var weatherForecast = Given.a_weather_forecast.with_date(DateTime.Today).with_summary("Cool").with_temperature(10).build();
            
            await _fixture.AddDatabaseItems(weatherForecast);

            var response = await _fixture.Get<WeatherForecastController, IEnumerable<WeatherForecast>>(controller => controller.Get(), Identities.Default);

            response.Should().ContainEquivalentOf(weatherForecast);
        }

        [Fact]
        public async Task get_by_id()
        {
            var weatherForecast = Given.a_weather_forecast.with_date(DateTime.Today).with_summary("Cool").with_temperature(10).build();

            await _fixture.AddDatabaseItems(weatherForecast);

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.Get(weatherForecast.Id), Identities.Default);

            response.Should().BeEquivalentTo(weatherForecast);
        }

        [Fact]
        public async Task create()
        {
            var request = new WeatherForecast { Date = DateTime.Today, Summary = "Cool", TemperatureC = 10 };

            var id = await _fixture.Post<WeatherForecastController, int>(controller => controller.Post(request), Identities.Default);

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.Get(id), Identities.Default);

            response.Should().BeEquivalentTo(request, options => options.Excluding(e => e.Id));
        }

        [Fact]
        public async Task update()
        {
            var weatherForecast = Given.a_weather_forecast.build();

            await _fixture.AddDatabaseItems(weatherForecast);
            
            var request = new WeatherForecast { Date = DateTime.Today.AddDays(1), Summary = "Updated", TemperatureC = 99 };

            await _fixture.Put<WeatherForecastController>(controller => controller.Put(weatherForecast.Id, request), Identities.Default);

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.Get(weatherForecast.Id), Identities.Default);

            response.Should().BeEquivalentTo(request, options => options.Excluding(e => e.Id));
        }

        [Fact]
        public async Task delete()
        {
            var weatherForecast = Given.a_weather_forecast.build();

            await _fixture.AddDatabaseItems(weatherForecast);
            
            var id = await _fixture.Post<WeatherForecastController, int>(controller => controller.Delete(weatherForecast.Id), Identities.Default);

            var response = await _fixture.Get<WeatherForecastController>(controller => controller.Get(id), Identities.Default);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}