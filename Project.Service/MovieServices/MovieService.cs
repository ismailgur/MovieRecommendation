using Project.Data.Repository;
using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Service.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            this._movieRepository = movieRepository;
        }
    }
}
