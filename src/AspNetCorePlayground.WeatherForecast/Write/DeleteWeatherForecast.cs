namespace AspNetCorePlayground.WeatherForecast.Write
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class DeleteWeatherForecast : IRequest
    {
        public int Id { get; }

        public DeleteWeatherForecast(int id)
        {
            Id = id;
        }
    }

    public sealed class DeleteWeatherForecastHandler : IRequestHandler<DeleteWeatherForecast>
    {
        private readonly DbContext _db;

        public DeleteWeatherForecastHandler(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<Unit> Handle(DeleteWeatherForecast request, CancellationToken cancellationToken)
        {
            var entity = await _db.Set<WeatherForecast>().FindAsync(request.Id);
            _db.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return default;
        }
    }
}