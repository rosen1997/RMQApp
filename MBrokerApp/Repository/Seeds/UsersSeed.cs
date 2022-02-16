using MBrokerApp.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBrokerApp.Repository.Seeds
{
    public static class UsersSeed
    {
        public static User[] Seed()
        {
            return new User[]
            {
                new User{Id=1, Name="Peter Petrov", Ucn="9206207963", Address="ul. Studentska", City="Varna", Country="Bulgaria", Gender=true, PhoneNumber="0875632908"}
            };
        }
    }
}
