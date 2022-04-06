using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Helpers;
using Project.Common.Helpers;
using Project.Data.Domain.Account;
using Project.Data.Dto;
using Project.Data.Dto.Account;
using Project.Service.Account;
using Project.Service.Logging;
using Project.Service.Security;
using System;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        #region Injections

        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ILogger _logger;

        #endregion


        #region ctor

        public AccountController(IUserService userService, ITokenStoreService tokenStoreService, ILogger logger)
        {
            this._userService = userService;
            this._tokenStoreService = tokenStoreService;
            this._logger = logger;
        }

        #endregion


        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var resultModel = new RequestStateDto();

            try
            {
                if (model == null)
                {
                    resultModel.Description = "Hatalı işlem";
                    return Ok(value: resultModel);
                }

                User user = await _userService.FindUserAsync(model.Username, model.Password).ConfigureAwait(false);

                if (user == null)
                {
                    resultModel.Description = "Kullanıcı bulunamadı";
                    return Ok(value: resultModel);
                }


                var (accessToken, refreshToken) = await _tokenStoreService.CreateJwtTokens(user).ConfigureAwait(false);

                resultModel.IsSuccess = true;
                resultModel.Data = new
                {
                    access_token = accessToken,
                    refresh_token = refreshToken,
                    user_id = user.Id,
                    display_name = $"{user.FirstName} {user.LastName}",
                };

                return Ok(value: resultModel);
            }
            catch (Exception err)
            {
                resultModel.Description = DebugHelper.GetExceptionErrorMessage(err);
                return Ok(value: resultModel);
            }
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] UserRegisterDto model)
        {
            var resultModel = new RequestStateDto();            

            try
            {
                #region validation

                if (model == null)
                {
                    resultModel.Description = "Hatalı işlem";
                    return Ok(value: resultModel);
                }

                if (!ModelState.IsValid)
                {
                    resultModel.Description = string.Join(",", ActionHelper.GetErrorListFromModelState(ModelState));
                    return Ok(value: resultModel);
                }

                #endregion


                this._userService.Insert(new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username
                });

                resultModel.IsSuccess = true;

                return Ok(value: resultModel);
            }
            catch (Exception err)
            {
                resultModel.Description = DebugHelper.GetExceptionErrorMessage(err);
                return Ok(value: resultModel);
            }
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("refreshToken is not set.");

            var token = await _tokenStoreService.FindTokenAsync(refreshToken);

            if (token == null || DateTime.UtcNow > token.RefreshTokenExpiresDateTime)
                return Ok(null);

            var (accessToken, newRefreshToken) = await _tokenStoreService.CreateJwtTokens(token.User).ConfigureAwait(false);
            return Ok(new { access_token = accessToken, refresh_token = newRefreshToken });
        }
    }
}