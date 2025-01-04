using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    internal interface IUserService
    {
        Task SaveUserAsync(User user);

        Task<List<User>> LoadUsersAsync();
    }
}
