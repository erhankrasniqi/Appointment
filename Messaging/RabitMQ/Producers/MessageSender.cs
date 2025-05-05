
using Messaging.RabbitMQ.Settings;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Messaging.RabitMQ.Producers
{
    public class MessageSender
    {
        private readonly RabbitMQConnection _rabbitMqConnection;

        public MessageSender(RabbitMQConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void SendMessage(string message, string queueName)
        {
            using (var channel = _rabbitMqConnection.CreateChannel()) 
            { 
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                 
                var body = Encoding.UTF8.GetBytes(message);
                 
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Message sent: {message}");
            }
        }
    }
}
