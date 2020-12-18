﻿namespace AspNetCorePlayground.WeatherForecast
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Read;
    using Read.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Write;
    using Write.Model;

    [ApiController]
    [Route("api/weather-forecasts")]
    public sealed class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WeatherForecastController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAll()
        {
            var result = await _mediator.Send(new GetWeatherForecasts());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WeatherForecast>> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetWeatherForecastById(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<CreatedAtActionResult> Create([FromBody] WeatherForecastDto model)
        {
            var id = await _mediator.Send(new CreateWeatherForecast(model));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] WeatherForecastDto model)
        {
            await _mediator.Send(new UpdateWeatherForecast(id, model));
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteWeatherForecast(id));
            return Ok();
        }
    }
}
