using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.MovieServices
{
    public interface IMovieService
    {
        Movie Insert(Movie model);
    }
}
