namespace AspNetCorePlayground.WeatherForecast.Read
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Write.Model;

    public sealed class GetWeatherForecastById : IRequest<WeatherForecastDto>
    {
        public int Id { get; }

        public GetWeatherForecastById(in int id)
        {
            Id = id;
        }
    }

    public sealed class GetWeatherForecastByIdHandler : IRequestHandler<GetWeatherForecastById, WeatherForecastDto>
    {
        private readonly DbContext _db;

        public GetWeatherForecastByIdHandler(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<WeatherForecastDto> Handle(GetWeatherForecastById request, CancellationToken cancellationToken)
        {
            return await _db.Set<WeatherForecast>()
                .AsNoTracking()
                .Where(e => e.Id == request.Id)
                .Select(e => new WeatherForecastDto
                {
                    Id = e.Id,
                    Date = e.Date,
                    TemperatureC = e.TemperatureC,
                    Summary = e.Summary
                }).SingleOrDefaultAsync(cancellationToken);
        }
    }
}