using Project.Data.Domain.Account;
using Project.Data.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Account
{
    public interface IUserService
    {
        User FindUser(long userId);

        Task<User> FindUserAsync(long userId);

        Task<User> FindUserAsync(string username, string password);

        Task UpdateUserLastActivityDateAsync(User user);

        bool AnyUsername(string username);

        User Insert(User model);

        User Update(User entity, bool withPassword = false);

        UserDto GetDto(long id);
    }
}
