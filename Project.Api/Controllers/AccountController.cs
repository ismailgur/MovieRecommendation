using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data.Domain.Account;
using Project.Data.Dto.Account;
using Project.Service.Account;
using Project.Service.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenStoreService;

        public AccountController(IUserService userService, ITokenStoreService tokenStoreService)
        {
            this._userService = userService;
            this._tokenStoreService = tokenStoreService;
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            try
            {
                if (model == null)
                    return BadRequest("user is not set.");

                User user = await _userService.FindUserAsync(model.Username, model.Password).ConfigureAwait(false);

                var (accessToken, refreshToken) = await _tokenStoreService.CreateJwtTokens(user).ConfigureAwait(false);

                return Ok(new
                {
                    access_token = accessToken,
                    refresh_token = refreshToken,
                    user_id = user.Id,
                    display_name = $"{user.FirstName} {user.LastName}",
                });
            }
            catch (Exception err)
            {
                throw;
            }
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] UserRegisterDto model)
        {
            try
            {
                #region validation

                if (model == null)
                    return BadRequest("user is not set.");

                if (string.IsNullOrEmpty(model.Username))
                    return BadRequest("Kullanıcı adı bilgisi hatalı!");

                if (string.IsNullOrEmpty(model.Password))
                    return BadRequest("Şifre bilgisi hatalı!");

                #endregion


                this._userService.Insert(new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username
                });


                return Ok(value: true);
            }
            catch (Exception err)
            {
                throw;
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