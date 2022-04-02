using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.IMDB
{
    public class IMDBService : IIMDBService
    {
        public IMDBService()
        {

        }


        // Tüm Filmler
        public void UpdateMovies()
        {
            string endPoint = "https://api.themoviedb.org/3/movie/now_playing?api_key=23818476598777d4bf155e9500fefb82";
        }


        // Yakında Eklenecek Olanlar
        public void UpdateUpComings()
        {
            string endPoint = "/movie/upcoming";
        }


        // Top 10
        public void UpdateTopRateds()
        {
            string endPoint = "/movie/top_rated";
        }

        
        public void GetMovieDetail(long id)
        {
            string endPoint = "/movie/{movie_id}";
        }


        public void GetRecommendations(long movieId)
        {
            string endPoint = "/movie/{movie_id}/recommendations";
        }
    }
}
