using MBrokerApp.Models;
using MBrokerApp.Repository.Entities;
using MBrokerApp.Repository.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserReceiverService> logger;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;
        public UserReceiverService(IOptions<RabbitMqConfiguration> rabbitMqOptions, IServiceProvider serviceProvider, ILogger<UserReceiverService> logger)
        {
            rabbitMqConfiguration = rabbitMqOptions.Value;
            InitializeRabbitMqListener();
            scope = serviceProvider.CreateScope();
            userManager = scope.ServiceProvider.GetService<IUserManager>();
            consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            this.logger = logger;
        }

        private void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var content = Encoding.UTF8.GetString(e.Body.ToArray());
            var user = JsonConvert.DeserializeObject<User>(content);

            try
            {
                userManager.CreateUser(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                ProccessError(e);
            }

            channel.BasicAck(e.DeliveryTag, false);
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConfiguration.Hostname,
                UserName = rabbitMqConfiguration.UserName,
                Password = rabbitMqConfiguration.Password,
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "UserErrorQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            channel.BasicConsume(rabbitMqConfiguration.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void ProccessError(BasicDeliverEventArgs e)
        {
            int numOfRetries = int.Parse(e.BasicProperties.Headers["numOfRetries"].ToString());

            if (numOfRetries == rabbitMqConfiguration.MaxRetries)
            {
                logger.LogInformation($"Message has been removed from UsersQueue");
                AddToErrorQueue(e.BasicProperties, e.Body);
                channel.BasicReject(e.DeliveryTag, false);
                return;
            }

            e.BasicProperties.Headers["numOfRetries"] = ++numOfRetries;

            channel.BasicPublish(exchange: "", routingKey: rabbitMqConfiguration.QueueName, basicProperties: e.BasicProperties, body: e.Body);
        }

        private void AddToErrorQueue(IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            properties.Headers.Add("timestamp", DateTime.UtcNow.ToString());
            channel.BasicPublish(exchange: "", routingKey: "UserErrorQueue", basicProperties: properties, body: body);
        }
    }
}
