namespace AspNetCorePlayground.WeatherForecast.Write.Model
{
    using System;

    public sealed class WeatherForecast
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
