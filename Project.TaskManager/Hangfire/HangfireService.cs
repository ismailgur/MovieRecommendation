using Microsoft.Extensions.Configuration;
using Project.Common.Helpers;
using Project.Data.Domain.MovieDomains;
using Project.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.TaskManager.Hangfire
{
    public class HangfireService : IHangfireService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Movie> _movieRepository;

        public HangfireService(IConfiguration configuration, IRepository<Movie> movieRepository)
        {
            this._configuration = configuration;
            this._movieRepository = movieRepository;
        }


        public async Task SyncMovies()
        {
            try
            {
                var now = DateTime.Now;

                string apiUrl = this._configuration["TheMovieDbSettings:ApiUrl"];
                string apiKey = this._configuration["TheMovieDbSettings:ApiKey"];

                var data = new Business.MovieIntegration.MovieAPI(apiUrl, apiKey).GetNowPlayingList();

                const int bulkInsertUpdateLimit = 500;
                var insertList = new List<Movie>();
                var updateList = new List<Movie>();


                foreach (var item in data)
                {
                    var entity = this._movieRepository.GetAll().Where(x => x.IntegrationId == item.id && !x.IsDeleted).OrderBy(x => x.Id).LastOrDefault();

                    // Ekle
                    if (entity == null)
                    {
                        insertList.Add(new Movie
                        {
                            Title = item.title,
                            OriginalTitle = item.original_title,
                            IntegrationId = item.id,
                            Overview = item.overview,
                            InsertDateTime = now
                        });


                        if (insertList.Count >= bulkInsertUpdateLimit)
                        {
                            this._movieRepository.AddRange(insertList);
                            insertList = new List<Movie>();
                        }
                    }

                    // Güncelle
                    else
                    {
                        var tempEntity = GenericHelper.Clone<Movie>(entity);

                        entity.Title = item.title;
                        entity.OriginalTitle = item.original_title;
                        entity.IntegrationId = item.id;
                        entity.Overview = item.overview;

                        if (!GenericHelper.Compare<Movie>(tempEntity, entity))
                        {
                            entity.UpdateDateTime = now;

                            updateList.Add(entity);

                            if (updateList.Count >= bulkInsertUpdateLimit)
                            {
                                this._movieRepository.UpdateRange(insertList);
                                updateList = new List<Movie>();
                            }
                        }

                    }
                }

                if (insertList.Count != 0)
                {
                    this._movieRepository.AddRange(insertList);
                    insertList = new List<Movie>();
                }

                if (updateList.Count != 0)
                {
                    this._movieRepository.UpdateRange(updateList);
                    updateList = new List<Movie>();
                }
            }
            catch (Exception err)
            {
                var msg = DebugHelper.GetExceptionErrorMessage(err);
                throw;
            }
        }
    }
}
