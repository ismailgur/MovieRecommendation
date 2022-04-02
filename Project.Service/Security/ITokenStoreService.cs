using Project.Data.Domain.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Security
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(UserToken userToken);

        Task AddUserTokenAsync(
                User user, string refreshToken, string accessToken,
                DateTimeOffset refreshTokenExpiresDateTime, DateTimeOffset accessTokenExpiresDateTime);

        Task<bool> IsValidTokenAsync(string accessToken, long userId);

        Task DeleteExpiredTokensAsync();

        Task<UserToken> FindTokenAsync(string refreshToken);

        Task DeleteTokenAsync(string refreshToken);

        Task InvalidateUserTokensAsync(long userId);

        Task<(string accessToken, string refreshToken)> CreateJwtTokens(User user);
    }
}
