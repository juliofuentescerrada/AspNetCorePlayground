namespace AspNetCorePlayground.WeatherForecast.Write
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using Read.Model;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateWeatherForecast : IRequest<int>
    {
        public WeatherForecastDto WeatherForecast { get; }

        public CreateWeatherForecast(WeatherForecastDto weatherForecast)
        {
            WeatherForecast = weatherForecast;
        }
    }
    
    public sealed class CreateWeatherForecastHandler : IRequestHandler<CreateWeatherForecast, int>
    {
        private readonly DbContext _db;

        public CreateWeatherForecastHandler(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<int> Handle(CreateWeatherForecast request, CancellationToken cancellationToken)
        {
            var entity = new WeatherForecast
            {
                Date = request.WeatherForecast.Date,
                TemperatureC = request.WeatherForecast.TemperatureC,
                Summary = request.WeatherForecast.Summary
            };
            
            await _db.Set<WeatherForecast>().AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}