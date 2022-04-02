using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Project.Common.Extensions;
using Project.Data.Domain.MovieDomains;
using Project.Data.Dto.MovieDtos;
using Project.Service.MovieServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
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
    }
}
