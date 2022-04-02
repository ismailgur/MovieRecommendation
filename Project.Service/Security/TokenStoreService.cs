using Microsoft.Extensions.Options;
using Project.Data.Domain.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Project.Common.Extensions;
using Project.Data.Repository;

namespace Project.Service.Security
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly ISecurityService _securityService;
        private readonly IRepository<UserToken> _userTokenRepository;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;


        public TokenStoreService(
            IRepository<UserToken> userTokenRepository,
            ISecurityService securityService,
            IOptionsSnapshot<BearerTokensOptions> configuration)
        {
            this._securityService = securityService;
            this._userTokenRepository = userTokenRepository;
            this._configuration = configuration;
        }

        public async Task AddUserTokenAsync(UserToken userToken)
        {
            await InvalidateUserTokensAsync(userToken.UserId).ConfigureAwait(false);
            this._userTokenRepository.Add(userToken);
        }


        public async Task AddUserTokenAsync(
                User user, string refreshToken, string accessToken,
                DateTimeOffset refreshTokenExpiresDateTime, DateTimeOffset accessTokenExpiresDateTime)
        {
            var token = new UserToken
            {
                UserId = user.Id,
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _securityService.GetSha256Hash(refreshToken),
                AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = refreshTokenExpiresDateTime,
                AccessTokenExpiresDateTime = accessTokenExpiresDateTime
            };
            await AddUserTokenAsync(token).ConfigureAwait(false);
        }


        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTimeOffset.UtcNow;
            var userTokens = await this._userTokenRepository.GetAll().Where(x => x.RefreshTokenExpiresDateTime < now).ToListAsync();
            foreach (var userToken in userTokens)
            {
                this._userTokenRepository.Delete(userToken);
            }
        }


        public async Task DeleteTokenAsync(string refreshToken)
        {
            var token = await FindTokenAsync(refreshToken).ConfigureAwait(false);
            if (token != null)
            {
                this._userTokenRepository.Delete(token);
            }
        }


        public Task<UserToken> FindTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }
            var refreshTokenIdHash = _securityService.GetSha256Hash(refreshToken);
            return this._userTokenRepository.GetAllIncluding(x => x.User).FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }


        public async Task InvalidateUserTokensAsync(long userId)
        {
            var userTokens = await this._userTokenRepository.GetAll().Where(x => x.UserId == userId).ToListAsync().ConfigureAwait(false);
            foreach (var userToken in userTokens)
            {
                this._userTokenRepository.Delete(userToken);
            }
        }


        public async Task<bool> IsValidTokenAsync(string accessToken, long userId)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            var userToken = await this._userTokenRepository.FindAsync(
                x => x.AccessTokenHash == accessTokenHash && x.UserId == userId).ConfigureAwait(false);
            //return userToken.AccessTokenExpiresDateTime >= DateTime.UtcNow;
            return userToken?.AccessTokenExpiresDateTime >= DateTime.UtcNow; // ismail değiştirdi
        }


        public async Task<(string accessToken, string refreshToken)> CreateJwtTokens(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var accessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes);
            var refreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes);
            var accessToken = await createAccessTokenAsync(user, accessTokenExpiresDateTime.UtcDateTime).ConfigureAwait(false);
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            await AddUserTokenAsync(user, refreshToken, accessToken, refreshTokenExpiresDateTime, accessTokenExpiresDateTime).ConfigureAwait(false);

            return (accessToken, refreshToken);
        }


        private async Task<string> createAccessTokenAsync(User user, DateTime expires)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("DisplayName", $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.UserData, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.SerialNumber, user.SerialNumber)
            };

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}