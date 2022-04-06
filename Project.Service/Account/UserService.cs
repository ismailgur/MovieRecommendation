using Project.Data.Domain.Account;
using Project.Data.Dto.Account;
using Project.Data.Repository;
using Project.Service.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Account
{
    public class UserService : IUserService
    {
        #region Injections

        private readonly IRepository<User> _userRepository;
        private readonly ISecurityService _securityService;

        #endregion


        #region ctor

        public UserService(IRepository<User> userRepository, ISecurityService securityService)
        {
            this._userRepository = userRepository;
            this._securityService = securityService;
        }

        #endregion


        public User FindUser(long userId)
        {
            return _userRepository.GetAll().Where(x => !x.IsDeleted && x.Id == userId).SingleOrDefault();
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
            if (user.LastLoggedIn != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }
            user.LastLoggedIn = DateTimeOffset.Now;
            await _userRepository.UpdateAsyn(user, user.Id).ConfigureAwait(false);
        }


        public bool AnyUsername(string username)
        {
            return this._userRepository.GetAll().Any(x => x.Username == username);
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


        public User Update(User entity, bool withPassword = false)
        {
            if (string.IsNullOrEmpty(entity.Username))
                throw new Exception("Kullanıcı adı boş bırakılamaz!");

            if (string.IsNullOrEmpty(entity.Password))
                throw new Exception("Kullanıcı şifresi boş bırakılamaz!");

            if (withPassword)
            {
                var passwordSalt = _securityService.GetSha256Hash(entity.Password);
                entity.Password = passwordSalt;
                entity.SerialNumber = Guid.NewGuid().ToString("N");
            }

            entity.UpdateDateTime = DateTime.Now;

            return this._userRepository.Update(entity, entity.Id);
        }


        public UserDto GetDto(long id)
        {
            return this._userRepository.GetAll().Where(x => x.Id == id && !x.IsDeleted).Select(x => new UserDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.Username
            })
                .SingleOrDefault();
        }
    }
}
