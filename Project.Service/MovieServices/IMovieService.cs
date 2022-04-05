﻿using Project.Data.Domain.MovieDomains;
using Project.Data.Dto.MovieDtos;
using System.Collections.Generic;

namespace Project.Service.MovieServices
{
    public interface IMovieService
    {
        MovieDetailDto GetMovieDetailByIntegrationId(long currentUserId, int id);

        double? GetRateScoreAvg(long movieId);

        IList<MovieDto> GetTop10List();

        IList<MovieDto> GetUpcomingList(int pageIndex, out int totalCount);

        MovieNote AddNoteByIntegrationId(long currentUserId, int integrationId, string note);

        bool ScoreUpdate(long currentUserId, int integrationId, int score, out long movieId);
    }
}
