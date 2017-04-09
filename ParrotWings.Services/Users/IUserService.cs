using ParrotWings.Models.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParrotWings.Services.Users
{
    public interface IUserService
    {
        User FindUserById(Guid id);
        Task<User> FindUserByIdAsync(Guid id);
        
        User FindUserByEmailOrName(string email, string name = "");
        Task<User> FindUserByEmailOrNameAsync(string email, string name = "");
        
        IEnumerable<User> GetUsersByName(string name);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name);
    }
}
