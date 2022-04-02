using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Project.Service.Account;

namespace Project.Service.Security
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenStoreService;


        public TokenValidatorService(IUserService userService, ITokenStoreService tokenStoreService)
        {
            _userService = userService;
            _tokenStoreService = tokenStoreService;
        }


        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                context.Fail("This is not our issued token. It has no serial.");
                return;
            }

            var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                context.Fail("This is not our issued token. It has no user-id.");
                return;
            }

            var user = await _userService.FindUserAsync(userId).ConfigureAwait(false);
            if (user == null || user.SerialNumber != serialNumberClaim.Value)
            {
                context.Fail("This token is expired. Please login again.");
            }

            var accessToken = context.SecurityToken as JwtSecurityToken;
            if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.RawData) ||
                !await _tokenStoreService.IsValidTokenAsync(accessToken.RawData, userId).ConfigureAwait(false))
            {
                context.Fail("This token is not in our database.");
                return;
            }

            await _userService.UpdateUserLastActivityDateAsync(user).ConfigureAwait(false);
        }
    }
}
