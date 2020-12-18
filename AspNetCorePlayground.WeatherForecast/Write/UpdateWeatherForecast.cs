namespace AspNetCorePlayground.WeatherForecast.Write
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using Read.Model;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class UpdateWeatherForecast : IRequest
    {
        public int Id { get; }
        public WeatherForecastDto Model { get; }

        public UpdateWeatherForecast(int id, WeatherForecastDto model)
        {
            Id = id;
            Model = model;
        }
    }

    public sealed class UpdateWeatherForecastHandler : IRequestHandler<UpdateWeatherForecast>
    {
        private readonly DbContext _db;

        public UpdateWeatherForecastHandler(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<Unit> Handle(UpdateWeatherForecast request, CancellationToken cancellationToken)
        {
            var entity = await _db.Set<WeatherForecast>().FindAsync(request.Id);
            entity.Date = request.Model.Date;
            entity.Summary = request.Model.Summary;
            entity.TemperatureC = request.Model.TemperatureC;
            await _db.SaveChangesAsync(cancellationToken);
            return default;
        }
    }
}