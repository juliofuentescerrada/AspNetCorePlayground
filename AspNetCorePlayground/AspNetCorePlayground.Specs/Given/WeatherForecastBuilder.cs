namespace AspNetCorePlayground.Specs.Given
{
    using Configuration;
    using Data.Model;
    using System;

    public class WeatherForecastBuilder
    {
        private DateTime _date;
        private string _summary;
        private int _temperature;

        public WeatherForecastBuilder with_date(DateTime date)
        {
            _date = date;
            return this;
        }

        public WeatherForecastBuilder with_summary(string summary)
        {
            _summary = summary;
            return this;
        }

        public WeatherForecastBuilder with_temperature(int temperature)
        {
            _temperature = temperature;
            return this;
        }

        public WeatherForecast build(TestFixture testFixture)
        {
            var i = new WeatherForecast
            {
                Date = _date,
                Summary = _summary,
                TemperatureC = _temperature
            };

            testFixture.AddDatabaseItems(i).ConfigureAwait(false).GetAwaiter().GetResult();

            return i;
        }

    }
}