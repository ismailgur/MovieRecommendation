using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Data.Domain.MovieDomains;
using Project.Service.MovieServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMovieService _movieService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            , IMovieService movieService)
        {
            _logger = logger;
            this._movieService = movieService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //this._movieService.Insert(new Data.Domain.MovieDomains.Movie
            //{
            //    Title = "test",
            //    InsertDateTime = DateTime.Now
            //});

            //_IMDBService.UpdateMovies();

            _movieService.Test();


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
