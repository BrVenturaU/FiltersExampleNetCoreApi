using FiltersExample.Filters;
using FiltersExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiltersExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [ServiceFilter(typeof(MetadaActionFilter))] Filtro de controlador
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionFilterExample))] // Filtro de acción
        public User Post([FromBody] User user)
        {
            return user;
        }

        [HttpPost("some")]
        [ServiceFilter(typeof(MetadaActionFilter))] // Filtro de acción
        public object Post([FromBody] Meta meta)
        {
            return new {
                some = meta.SomeProp,
                tr = meta.TransactionId,
                app = meta.ApplicationSource
            };
        }
    }
}