using Project.Data.Repository;
using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            this._movieRepository = movieRepository;
        }


        public Movie Insert(Movie model)
        {
            return this._movieRepository.Add(model);
        }

        public Movie Update(Movie entity)
        {
            return this._movieRepository.Update(entity,entity.Id);
        }
    }
}
