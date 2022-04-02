using Project.Data.Domain.Account;
using Project.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Account
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }


        public Task<User> FindUserAsync(long userId)
        {
            return _userRepository.GetAsync(userId);
        }


        public async Task UpdateUserLastActivityDateAsync(User user)
        {
            //var user = await FindUserAsync(userId).ConfigureAwait(false);
            //if (user.LastLoggedIn != null)
            //{
            //    var updateLastActivityDate = TimeSpan.FromMinutes(2);
            //    var currentUtc = DateTimeOffset.UtcNow;
            //    var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
            //    if (timeElapsed < updateLastActivityDate)
            //    {
            //        return;
            //    }
            //}
            //user.LastLoggedIn = DateTimeOffset.UtcNow;
            ////await _userRepository.UpdateAsyn(user, userId).ConfigureAwait(false);
            //await _userRepository.SetColumnValueAsync("LastLoggedIn", user.Id, user.LastLoggedIn).ConfigureAwait(false);
        }
    }
}
