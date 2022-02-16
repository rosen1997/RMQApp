using MBrokerApp.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBrokerApp.Repository.Managers
{
    public class UserManager : IUserManager
    {
        private readonly RepositoryContext repositoryContext;

        public UserManager(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public void CreateUser(User user)
        {
            repositoryContext.Users.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return repositoryContext.Users;
        }
    }
}
