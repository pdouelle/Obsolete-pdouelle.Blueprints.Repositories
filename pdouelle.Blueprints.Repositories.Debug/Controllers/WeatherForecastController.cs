using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace pdouelle.Blueprints.Repositories.Debug.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepository<WeatherForecast> _repository;

        public WeatherForecastController(IRepository<WeatherForecast> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            IQueryable<WeatherForecast> queryable = _repository.GetAll();

            List<WeatherForecast> entities = await queryable.ToListAsync(cancellationToken);

            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            WeatherForecast entity = await _repository.GetByIdAsync(id, cancellationToken);
            
            if (entity is null) return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] WeatherForecast resource, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(resource, cancellationToken);

            await _repository.SaveAsync(cancellationToken);

            return Created($"{HttpContext.Request.Path}/{resource.Id}", resource);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, [FromBody] JsonPatchDocument<WeatherForecast> resourcePatch, CancellationToken cancellationToken)
        {
            WeatherForecast entity = await _repository.GetByIdAsync(id, cancellationToken);
            
            if (entity is null) return NotFound();
            
            resourcePatch.ApplyTo(entity);
            
            _repository.Update(entity);
            
            await _repository.SaveAsync(cancellationToken);

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            WeatherForecast entity = await _repository.GetByIdAsync(id, cancellationToken);

            if (entity is null) return NotFound();

            _repository.Remove(entity);

            await _repository.SaveAsync(cancellationToken);

            return NoContent();
        }
    }
}