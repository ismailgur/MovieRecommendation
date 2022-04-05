using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Extensions;
using Project.Data.Dto;
using Project.Data.Dto.MovieDtos;
using Project.Service.MovieServices;
using System.Collections.Generic;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [EnableCors("CorsPolicy")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;


        public MovieController(IMovieService movieService)
        {
            this._movieService = movieService;
        }


        [HttpGet("[action]")]
        public IEnumerable<MovieDto> GetTop10List()
        {
            var data = this._movieService.GetTop10List();

            return data;
        }


        [HttpGet("[action]")]
        public IActionResult GetUpcomingList(int page)
        {
            var data = this._movieService.GetUpcomingList(page, out int totalCount);

            var res = new { data = data, totalElements = totalCount };

            return Ok(res);
        }


        [HttpGet("[action]")]
        public MovieDetailDto GetDetailByIntegrationId(int id)
        {
            var currentUser = HttpContext.User.GetCurrentUser();

            var data = this._movieService.GetMovieDetailByIntegrationId(currentUser.UserId, id);

            return data;
        }


        [HttpPost("[action]")]
        public IActionResult AddNoteSave([FromBody] MovieNoteSaveDto model)
        {
            var currentUser = HttpContext.User.GetCurrentUser();

            var resultModel = new RequestStateDto();

            #region validation

            if (model == null)
            {
                resultModel.Description = "Hatalı işlem";
                return Ok(value: resultModel);
            }

            if (string.IsNullOrEmpty(model.Note) || model.Note.Length < 3)
            {
                resultModel.Description = "Not içeriği çok kısa";
                return Ok(value: resultModel);
            }

            #endregion


            var entity = this._movieService.AddNoteByIntegrationId(currentUser.UserId, model.IntegrationId, model.Note);

            if (entity == null)
            {
                resultModel.Description = "Kayıt eklenemedi";
                return Ok(value: resultModel);
            }

            resultModel.IsSuccess = true;
            resultModel.Data = new MovieNoteDto
            {
                Id = entity.Id,
                InsertDateTime = entity.InsertDateTime,
                Note = entity.Note
            };

            return Ok(value: resultModel);
        }


        [HttpPost("[action]")]
        public IActionResult ScoreUpdate([FromBody] MovieScoreSaveDto model)
        {
            var currentUser = HttpContext.User.GetCurrentUser();

            var resultModel = new RequestStateDto();

            #region validation

            if (model == null)
            {
                resultModel.Description = "Hatalı işlem";
                return Ok(value: resultModel);
            }

            if (model.Score < 1 || model.Score > 10)
            {
                resultModel.Description = "Skor aralığı 1 ile 10 olabilir";
                return Ok(value: resultModel);
            }

            #endregion

            var result = this._movieService.ScoreUpdate(currentUser.UserId, model.IntegrationId, model.Score, out long movieId);

            if(result == false)
            {
                resultModel.Description = "Kayıt eklenemedi";
                return Ok(value: resultModel);
            }

            var rateScoreAvg = this._movieService.GetRateScoreAvg(movieId);

            resultModel.IsSuccess = true;
            resultModel.Data = rateScoreAvg;            

            return Ok(value: resultModel);
        }
    }
}
