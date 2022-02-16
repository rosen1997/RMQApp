using MBrokerApp.Models;
using MBrokerApp.Repository.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBrokerApp.Services
{
    public class UserSenderService : IUserSenderService
    {
        private readonly RabbitMqConfiguration rabbitMqConfiguration;
        private IConnection connection;
        public UserSenderService(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            rabbitMqConfiguration = rabbitMqOptions.Value;
            CreateConnection();
        }

        public void SendCustomer(User user)
        {
            if (ConnectionExists())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(user);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: rabbitMqConfiguration.QueueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitMqConfiguration.Hostname,
                    UserName = rabbitMqConfiguration.UserName,
                    Password = rabbitMqConfiguration.Password
                };
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (connection != null)
            {
                return true;
            }

            CreateConnection();

            return connection != null;
        }
    }
}
