namespace AspNetCorePlayground.Specs.Specs
{
    using Configuration;
    using Extensions;
    using FluentAssertions;
    using Given;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using WeatherForecast;
    using WeatherForecast.Read.Model;
    using WeatherForecast.Write.Model;
    using Xunit;

    [Collection(nameof(TestFixture))]
    public sealed class weather_forecast_controller_should : IAsyncLifetime
    {
        private readonly TestFixture _fixture;
        public Task InitializeAsync() => _fixture.ResetDatabase();
        public Task DisposeAsync() => Task.CompletedTask;

        public weather_forecast_controller_should(TestFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task get_all_weather_forecasts()
        {
            var weatherForecast = Given.a_weather_forecast
                .with_date(DateTime.Today)
                .with_summary("Cool")
                .with_temperature(10)
                .build();

            await _fixture.AddToDatabase(weatherForecast);

            var response = await _fixture.Get<WeatherForecastController, IEnumerable<WeatherForecast>>(controller => controller.GetAll());

            response.Should().ContainEquivalentOf(weatherForecast);
        }

        [Fact]
        public async Task get_a_single_forecast_by_id()
        {
            var weatherForecast = Given.a_weather_forecast
                .with_date(DateTime.Today)
                .with_summary("Cool")
                .with_temperature(10)
                .build();

            await _fixture.AddToDatabase(weatherForecast);

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.GetById(weatherForecast.Id));

            response.Should().BeEquivalentTo(weatherForecast);
        }

        [Fact]
        public async Task create_a_new_weather_forecast()
        {
            var request = new WeatherForecastDto { Date = DateTime.Today, Summary = "Cool", TemperatureC = 10 };

            var id = await _fixture.Post<WeatherForecastController, int>(controller => controller.Create(request));

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.GetById(id));

            response.Should().BeEquivalentTo(request, options => options
                .Excluding(e => e.Id)
                .ExcludingMissingMembers());
        }

        [Fact]
        public async Task update_an_existing_weather_forecast()
        {
            var weatherForecast = Given.a_weather_forecast.build();

            await _fixture.AddToDatabase(weatherForecast);

            var request = new WeatherForecastDto { Date = DateTime.Today.AddDays(1), Summary = "Updated", TemperatureC = 99 };

            await _fixture.Put<WeatherForecastController>(controller => controller.Update(weatherForecast.Id, request));

            var response = await _fixture.Get<WeatherForecastController, WeatherForecast>(controller => controller.GetById(weatherForecast.Id));

            response.Should().BeEquivalentTo(request, options => options
                .Excluding(e => e.Id)
                .ExcludingMissingMembers());
        }

        [Fact]
        public async Task delete_an_existing_weather_forecast()
        {
            var weatherForecast = Given.a_weather_forecast.build();

            await _fixture.AddToDatabase(weatherForecast);

            await _fixture.Delete<WeatherForecastController>(controller => controller.Delete(weatherForecast.Id));

            var response = await _fixture.Get<WeatherForecastController>(controller => controller.GetById(weatherForecast.Id));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}