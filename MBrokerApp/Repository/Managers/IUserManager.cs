using MBrokerApp.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBrokerApp.Repository.Managers
{
    public interface IUserManager
    {
        void CreateUser(User user);
        IEnumerable<User> GetAllUsers();
    }
}
