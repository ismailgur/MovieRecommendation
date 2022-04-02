using Project.Data.Repository;
using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Project.Data.Dto.MovieDtos;

namespace Project.Service.MovieServices
{
    public class MovieService : IMovieService
    {
        #region Declaretions

        private string IntegrationApiUrl;
        private string IntegrationApiKey;

        #endregion


        #region Injections

        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<MovieNote> _movieNoteRepository;
        private readonly IRepository<MovieRate> _movieRateRepository;

        #endregion


        #region ctor

        public MovieService(
            IConfiguration configuration
            , IRepository<Movie> movieRepository
            , IRepository<MovieNote> movieNoteRepository
            , IRepository<MovieRate> movieRateRepository)
        {
            this._movieRepository = movieRepository;
            this._movieNoteRepository = movieNoteRepository;
            this._movieRateRepository = movieRateRepository;

            this.IntegrationApiUrl = configuration["TheMovieDbSettings:ApiUrl"];
            this.IntegrationApiKey = configuration["TheMovieDbSettings:ApiKey"];
        }
        #endregion


        public MovieDetailDto GetMovieDetailByIntegrationId(long currentUserId, int id)
        {
            var apiClient = new Business.MovieIntegration.MovieAPI(this.IntegrationApiUrl, this.IntegrationApiKey);
            var item = apiClient.GetDetailById(id);

            var entity = this._movieRepository.GetAll().Where(x => x.IntegrationId == item.id && !x.IsDeleted).OrderBy(x => x.Id).LastOrDefault();

            if (entity == null) // insert if not exists on db
            {
                entity = this._movieRepository.Add(new Movie
                {
                    InsertDateTime = DateTime.Now,
                    OriginalTitle = item.original_title,
                    Overview = item.overview,
                    Title = item.title,
                    IntegrationId = item.id
                });
            }

            var rates = this._movieRateRepository.GetAll().Where(r => r.MovieId == entity.Id).ToList();


            var model = new MovieDetailDto
            {
                Id = entity.Id,
                Title = entity.Title,
                IntegrationId = entity.IntegrationId,

                Notes = this._movieNoteRepository.GetAll().Where(n => !n.IsDeleted && n.UserId == currentUserId && n.MovieId == entity.Id)
                .Select(n => new MovieNoteDto
                {
                    Id = n.Id,
                    Note = n.Note,
                    InsertDateTime = n.InsertDateTime
                }).OrderByDescending(n => n.InsertDateTime).ToList(),

                RateScore = rates.Where(r => r.UserId == currentUserId).SingleOrDefault()?.Score,
                RateScoreAvg = rates.Any() ? rates.Select(r => r.Score).Average() : null
            };

            return model;
        }


        public IList<MovieDto> GetTop10List()
        {
            var apiClient = new Business.MovieIntegration.MovieAPI(this.IntegrationApiUrl, this.IntegrationApiKey);

            var integrationData = apiClient.GetTop10List();

            var result = integrationData.Select(x => new MovieDto
            {
                Title = x.title,
                IntegrationId = x.id
            }).ToList();

            return result;
        }


        public IList<MovieDto> GetUpcomingList(int pageIndex, out int totalCount)
        {
            var apiClient = new Business.MovieIntegration.MovieAPI(this.IntegrationApiUrl, this.IntegrationApiKey);

            var integrationData = apiClient.GetUpcomingList(pageIndex, out totalCount);

            var result = integrationData.Select(x => new MovieDto
            {
                Title = x.title,
                IntegrationId = x.id
            }).ToList();

            return result;
        }

    }
}
