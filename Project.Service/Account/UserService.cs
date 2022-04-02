using Project.Data.Domain.Account;
using Project.Data.Repository;
using Project.Service.Security;
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
        private readonly ISecurityService _securityService;


        public UserService(IRepository<User> userRepository, ISecurityService securityService)
        {
            this._userRepository = userRepository;
            this._securityService = securityService;
        }


        public Task<User> FindUserAsync(long userId)
        {
            return _userRepository.GetAsync(userId);
        }


        public Task<User> FindUserAsync(string username, string password)
        {
            var passwordHash = _securityService.GetSha256Hash(password);
            return _userRepository.FindAsync(x => x.Username == username && x.Password == passwordHash && !x.IsDeleted);
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


        public User Insert(User model)
        {
            if (string.IsNullOrEmpty(model.Username))
                throw new Exception("Kullanıcı adı boş bırakılamaz!");

            if (string.IsNullOrEmpty(model.Password))
                throw new Exception("Kullanıcı şifresi boş bırakılamaz!");

            var passwordSalt = _securityService.GetSha256Hash(model.Password);
            model.Password = passwordSalt;
            model.SerialNumber = Guid.NewGuid().ToString("N");
            model.InsertDateTime = DateTime.Now;

            return this._userRepository.Add(model);
        }
    }
}
