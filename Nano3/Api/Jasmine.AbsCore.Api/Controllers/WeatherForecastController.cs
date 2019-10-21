using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jasmine.AbsCore.Entities.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jasmine.AbsCore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherForecastRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IWeatherForecastRepository repository,
            ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _repository.GetAll();
        }
    }


    public interface IWeatherForecastRepository
    {
        IEnumerable<WeatherForecast> GetAll();
    }


    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        public WeatherForecastRepository(AbsContext context)
        {
            _context = context;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly AbsContext _context;

        public IEnumerable<WeatherForecast> GetAll()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
