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
        private const int MaximumMovie = 10000;
        private const int resultPageSize = 20; // api den dönen resultta sayfa başına gelen kayıt sayısı. değiştirilmeyecek.

        public MovieAPI(string apiAddress, string apiKey)
        {
            this._apiAddresss = apiAddress;
            this._apiKey = apiKey;
        }


        private string GetRestClientResponse(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = 10000; // 10000ms = 10s

            var request = new RestRequest();

            request.Parameters.Clear();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("cache-control", "no-cache");

            return client.Execute(request).Content;
        }

        private MovieApiResultModel<T> RequestWithPage<T>(string endPoint, int page)
        {
            var uriBuilder = new UriBuilder(Path.Combine(_apiAddresss, endPoint));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["api_key"] = _apiKey;
            query["page"] = page.ToString();
            uriBuilder.Query = query.ToString();

            var response = GetRestClientResponse(uriBuilder.ToString());

            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieApiResultModel<T>>(response);

            return responseObject;
        }



        public List<MovieApiMovieModel> GetMovies()
        {
            var list = new List<MovieApiMovieModel>();

            for (int i = 1; i <= (MaximumMovie / resultPageSize) + (MaximumMovie % resultPageSize != 0 ? 1 : 0); i++)
            {
                Console.WriteLine($"page:{i}");

                var responseObject = RequestWithPage<MovieApiMovieModel>("discover/movie", i);

                list.AddRange(responseObject.results);

                if (responseObject.results.Count == 0)
                    break;
            }

            return list.Take(MaximumMovie).ToList();
        }


        public List<MovieApiMovieModel> GetUpcomingList(int page, out int totalResultsCount)
        {
            var responseObject = RequestWithPage<MovieApiMovieModel>("movie/upcoming", page);
            totalResultsCount = responseObject.total_results;
            return responseObject.results;
        }


        public List<MovieApiMovieModel> GetTop10List()
        {
            var responseObject = RequestWithPage<MovieApiMovieModel>("movie/top_rated", 1);
            return responseObject.results.Take(10).ToList();
        }


        public MovieApiMovieModel GetDetailById(int id)
        {
            var uriBuilder = new UriBuilder(Path.Combine(_apiAddresss, $"movie/{id}"));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["api_key"] = _apiKey;
            uriBuilder.Query = query.ToString();

            var response = GetRestClientResponse(uriBuilder.ToString());

            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieApiMovieModel>(response);

            return responseObject;
        }
    }
}
