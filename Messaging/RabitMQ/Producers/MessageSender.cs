
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
            using (var channel = _rabbitMqConnection.CreateChannel()) // Ensure channel is closed after use
            {
                // Krijo një queue nëse nuk ekziston
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                // Krijo mesazhin në formatin byte array
                var body = Encoding.UTF8.GetBytes(message);

                // Dërgo mesazhin në RabbitMQ
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Message sent: {message}");
            }
        }
    }
}
