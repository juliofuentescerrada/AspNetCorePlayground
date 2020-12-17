namespace AspNetCorePlayground.Controllers
{
    using Data;
    using Data.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/weather-forecasts")]
    public sealed class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastDbContext _db;

        public WeatherForecastController(WeatherForecastDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            var result = await _db.WeatherForecasts.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WeatherForecast>> Get([FromRoute] int id)
        {
            return await _db.WeatherForecasts.FindAsync(id);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> Post([FromBody] WeatherForecast model)
        {
            await _db.WeatherForecasts.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), model.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] WeatherForecast model)
        {
            var entity = await _db.WeatherForecasts.FindAsync(id);
            entity.Date = model.Date;
            entity.Summary = model.Summary;
            entity.TemperatureC = model.TemperatureC;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await _db.WeatherForecasts.FindAsync(id);
            _db.Remove(entity);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
