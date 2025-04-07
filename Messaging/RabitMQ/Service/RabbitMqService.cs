
using Messaging.RabbitMQ.Settings;
using Messaging.RabitMQ.Interfaces; 
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Messaging.RabbitMQ.Service
{
    public class RabbitMqService : IMessagePublisher, IMessageSubscriber, IDisposable
    {
        private readonly RabbitMqSettings _settings;
        private readonly RabbitMQConnection _rabbitConnection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMqSettings");
            _settings = section.Get<RabbitMqSettings>();


            _rabbitConnection = new RabbitMQConnection(_settings.HostName, _settings.QueueName, _settings.ExchangeName);
            _channel = _rabbitConnection.CreateChannel();
        }

        public void PublishMessage(string message, string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void SubscribeToQueue(string queueName, Action<string> callback)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _rabbitConnection?.Dispose();
        }
    }
}
