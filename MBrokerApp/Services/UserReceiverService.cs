using MBrokerApp.Models;
using MBrokerApp.Repository.Entities;
using MBrokerApp.Repository.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MBrokerApp.Services
{
    public class UserReceiverService : BackgroundService
    {
        private readonly RabbitMqConfiguration rabbitMqConfiguration;
        private readonly IServiceScope scope;
        private readonly IUserManager userManager;
        private IConnection connection;
        private IModel channel;
        public UserReceiverService(IOptions<RabbitMqConfiguration> rabbitMqOptions, IServiceProvider serviceProvider)
        {
            rabbitMqConfiguration = rabbitMqOptions.Value;
            InitializeRabbitMqListener();
            scope = serviceProvider.CreateScope();
            userManager = scope.ServiceProvider.GetService<IUserManager>();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConfiguration.Hostname,
                UserName = rabbitMqConfiguration.UserName,
                Password = rabbitMqConfiguration.Password
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var user = JsonConvert.DeserializeObject<User>(content);

                userManager.CreateUser(user);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(rabbitMqConfiguration.QueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
