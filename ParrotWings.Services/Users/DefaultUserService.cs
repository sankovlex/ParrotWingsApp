using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Data.Core.Repository;
using System.Linq;
using ParrotWings.Data.Extensions;

namespace ParrotWings.Services.Users
{
    public class DefaultUserService : IUserService
    {
        private IRepository<User> userRepository;

        public DefaultUserService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public User FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await userRepository.FindAsync(x => x.Email == email);
        }

        public User FindUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindUserByIdAsync(Guid id)
        {
            return await userRepository.FindAsync(x => x.UserId == id);
        }

        public IEnumerable<User> GetUsersByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
        {
            return await userRepository.GetAsync(q => q.Where(x => x.Name.Contains(name)));
        }
    }
}
