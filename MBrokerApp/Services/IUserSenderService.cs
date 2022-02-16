using MBrokerApp.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBrokerApp.Services
{
    public interface IUserSenderService
    {
        void SendCustomer(User user);
    }
}
