﻿using Project.Data.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Account
{
    public interface IUserService
    {
        Task<User> FindUserAsync(long userId);

        Task UpdateUserLastActivityDateAsync(User user);
    }
}