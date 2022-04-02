using Project.Data.Domain.MovieDomains;
using Project.Data.Dto.MovieDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.MovieServices
{
    public interface IMovieService
    {
        MovieDetailDto GetMovieDetailByIntegrationId(long currentUserId, int id);

        IList<MovieDto> GetTop10List();

        IList<MovieDto> GetUpcomingList(int pageIndex, out int totalCount);
    }
}
