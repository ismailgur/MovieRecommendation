using Project.Data.Repository;
using Project.Data.Domain.MovieDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Common.Helpers;

namespace Project.Service.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            this._movieRepository = movieRepository;
        }

        public Movie FindByIntegrationId(long id)
        {
            return this._movieRepository.GetAll().OrderBy(x=>x.Id).LastOrDefault(x => x.IntegrationId == id && !x.IsDeleted);
        }


        public Movie Insert(Movie model)
        {
            return this._movieRepository.Add(model);
        }

        public Movie Update(Movie entity)
        {
            return this._movieRepository.Update(entity, entity.Id);
        }

        public void Test()
        {
            var now = DateTime.Now;

            var data = new Business.MovieIntegration.MovieAPI("https://api.themoviedb.org/3", "23818476598777d4bf155e9500fefb82").GetNowPlayingList();

            const int bulkInsertUpdateLimit = 500;
            var insertList = new List<Movie>();
            var updateList = new List<Movie>();


            foreach (var item in data)
            {
                var entity = this.FindByIntegrationId(item.id);

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

                    if (!GenericHelper.Compare<Movie>(tempEntity,entity))
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
    }
}
