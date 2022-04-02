using Microsoft.AspNetCore.Mvc;
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
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            this._movieService = movieService;
        }

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            //this._movieService.Insert(new Data.Domain.MovieDomains.Movie
            //{
            //    Title = "test",
            //    InsertDateTime = DateTime.Now
            //});

            //_IMDBService.UpdateMovies();

            return null;
        }
    }
}
