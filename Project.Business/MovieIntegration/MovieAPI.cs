using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project.Business.MovieIntegration
{
    public class MovieAPI
    {
        private string _apiAddresss;
        private string _apiKey;
        private const int MaximumMovie = 101;

        public MovieAPI(string apiAddress, string apiKey)
        {
            this._apiAddresss = apiAddress;
            this._apiKey = apiKey;
        }


        private List<T> RequestPaginationResult<T>(string endPoint, int dataSize)
        {
            const int resultPageSize = 20; // api den dönen resultta sayfa başına gelen kayıt sayısı.

            var list = new List<T>();

            for (int i = 1; i <= (dataSize / resultPageSize) + (dataSize % resultPageSize != 0 ? 1 : 0); i++)
            {
                var uriBuilder = new UriBuilder(Path.Combine(_apiAddresss, endPoint));
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["api_key"] = _apiKey;
                query["page"] = i.ToString();
                uriBuilder.Query = query.ToString();

                var client = new RestClient(uriBuilder.ToString());
                client.Timeout = 5000; // 5000ms = 5s

                var request = new RestRequest();

                request.Parameters.Clear();
                request.Method = Method.GET;
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("cache-control", "no-cache");

                var response = client.Execute(request).Content;

                var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieApiResultModel<T>>(response);
                list.AddRange(responseObject.results);
            }

            list = list.Take(dataSize).ToList();

            return list;
        }


        public List<MovieApiMovieModel> GetNowPlayingList()
        {
            var data = RequestPaginationResult<MovieApiMovieModel>("movie/now_playing", MaximumMovie);
            return data;
        }


        public List<MovieApiMovieModel> GetUpcomingList()
        {
            var data = RequestPaginationResult<MovieApiMovieModel>("movie/upcoming", MaximumMovie);
            return data;
        }


        public List<MovieApiMovieModel> GetTop10List()
        {
            var data = RequestPaginationResult<MovieApiMovieModel>("movie/top_rated", 10);
            return data;
        }
    }
}
