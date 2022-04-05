using Project.Data.Repository;
using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Project.Data.Dto.MovieDtos;
using System.IO;

namespace Project.Service.MovieServices
{
    public class MovieService : IMovieService
    {
        #region Declaretions

        private string IntegrationApiUrl;
        private string IntegrationApiKey;
        private string IntegrationCdnAddress;

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
            this.IntegrationCdnAddress = configuration["TheMovieDbSettings:CdnUrl"];
        }
        #endregion


        public MovieDetailDto GetMovieDetailByIntegrationId(long currentUserId, int id)
        {
            var apiClient = new Business.MovieIntegration.MovieAPI(this.IntegrationApiUrl, this.IntegrationApiKey);
            var item = apiClient.GetDetailById(id);

            if (item.id == 0)
                return null;

            var entity = this._movieRepository.GetAll().Where(x => x.IntegrationId == item.id && !x.IsDeleted).OrderBy(x => x.Id).LastOrDefault();

            if (entity == null) // insert if not exists on db
            {
                entity = this._movieRepository.Add(new Movie
                {
                    InsertDateTime = DateTime.Now,
                    OriginalTitle = item.original_title,
                    Overview = item.overview,
                    Title = item.title,
                    IntegrationId = item.id,
                    BackDropImagePath = item.backdrop_path,
                    PosterImagePath = item.poster_path
                });
            }


            var model = new MovieDetailDto
            {
                Id = entity.Id,
                Title = entity.Title,
                OriginalTitle = entity.OriginalTitle,
                IntegrationId = entity.IntegrationId,
                Image = $"{this.IntegrationCdnAddress}{entity.PosterImagePath}",
                TopImage = $"{this.IntegrationCdnAddress}{entity.BackDropImagePath}",
                Overview = entity.Overview,

                Notes = this._movieNoteRepository.GetAll().Where(n => !n.IsDeleted && n.UserId == currentUserId && n.MovieId == entity.Id)
                .Select(n => new MovieNoteDto
                {
                    Id = n.Id,
                    Note = n.Note,
                    InsertDateTime = n.InsertDateTime,
                }).OrderByDescending(n => n.InsertDateTime).ToList(),

                RateScore = this._movieRateRepository.GetAll().Where(r => r.MovieId == entity.Id && r.UserId == currentUserId).SingleOrDefault()?.Score,
                RateScoreAvg = GetRateScoreAvg(entity.Id)
            };

            return model;
        }


        public double? GetRateScoreAvg(long movieId)
        {
            var rates = this._movieRateRepository.GetAll().Where(r => r.MovieId == movieId).Select(x => x.Score).ToList();
            return rates.Any() ? Math.Round(rates.Average(),1) : null;
        }


        public IList<MovieDto> GetTop10List()
        {
            var apiClient = new Business.MovieIntegration.MovieAPI(this.IntegrationApiUrl, this.IntegrationApiKey);

            var integrationData = apiClient.GetTop10List();

            var result = integrationData.Select(x => new MovieDto
            {
                Title = x.title,
                OriginalTitle = x.original_title,
                IntegrationId = x.id,
                Image = $"{this.IntegrationCdnAddress}{x.poster_path}",
                TopImage = $"{this.IntegrationCdnAddress}{x.backdrop_path}",
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
                OriginalTitle = x.original_title,
                IntegrationId = x.id,
                Image = $"{this.IntegrationCdnAddress}{x.poster_path}",
                TopImage = $"{this.IntegrationCdnAddress}{x.backdrop_path}",
            }).ToList();

            return result;
        }


        public MovieNote AddNoteByIntegrationId(long currentUserId, int integrationId, string note)
        {
            var movieEntityId = this._movieRepository.GetAll().Where(x => x.IntegrationId == integrationId && !x.IsDeleted).Select(x => x.Id).OrderBy(x => x).LastOrDefault();

            if (movieEntityId == 0)
                return null;

            var entity = this._movieNoteRepository.Add(new MovieNote
            {
                MovieId = movieEntityId,
                UserId = currentUserId,
                Note = note,
                InsertDateTime = DateTime.Now
            });

            return entity;
        }


        public bool ScoreUpdate(long currentUserId, int integrationId, int score, out long movieId)
        {
            var movieEntityId = this._movieRepository.GetAll().Where(x => x.IntegrationId == integrationId && !x.IsDeleted).Select(x => x.Id).OrderBy(x => x).LastOrDefault();
            movieId = movieEntityId;

            if (movieEntityId == 0)
                return false;

            var scoreEntity = this._movieRateRepository.GetAll().Where(r => r.MovieId == movieEntityId && r.UserId == currentUserId).SingleOrDefault();
            
            if (scoreEntity == null)
            {
                this._movieRateRepository.Add(new MovieRate { MovieId = movieEntityId, UserId = currentUserId, Score = score });
            }
            else
            {
                if (scoreEntity.Score != score)
                {
                    scoreEntity.Score = score;
                    this._movieRateRepository.Update(scoreEntity, new object[] {currentUserId, movieEntityId });
                }
            }

            return true;
        }
    }
}
