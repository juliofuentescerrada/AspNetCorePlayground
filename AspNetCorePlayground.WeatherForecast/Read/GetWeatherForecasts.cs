namespace AspNetCorePlayground.WeatherForecast.Read
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Write.Model;

    public sealed class GetWeatherForecasts : IRequest<IEnumerable<WeatherForecastDto>>
    {
    }

    public sealed class GetWeatherForecastsHandler : IRequestHandler<GetWeatherForecasts, IEnumerable<WeatherForecastDto>>
    {
        private readonly DbContext _db;

        public GetWeatherForecastsHandler(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<WeatherForecastDto>> Handle(GetWeatherForecasts request, CancellationToken cancellationToken)
        {
            return await _db.Set<WeatherForecast>()
                .AsNoTracking()
                .Select(e => new WeatherForecastDto
                {
                    Id = e.Id,
                    Date = e.Date,
                    TemperatureC = e.TemperatureC,
                    Summary = e.Summary
                }).ToListAsync(cancellationToken);
        }
    }
}