using Messaging.RabbitMQ.Settings;
using Messaging.RabbitMQ.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Messaging.RabitMQ.Consumers
{
    public class MessageReceiver
    {
        private readonly RabbitMQConnection _rabbitMqConnection;

        public MessageReceiver(RabbitMQConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void ReceiveMessage(string queueName)
        {
            // Create the channel outside the using block to have full control over its lifecycle
            var channel = _rabbitMqConnection.CreateChannel();

            try
            {
                // Sigurohu që queue është deklaruar
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Message received: {message}");
                };

                // Filloni konsumimin e mesazheve
                channel.BasicConsume(queueName, autoAck: true, consumer: consumer);

                Console.WriteLine($"Waiting for messages in {queueName}. To exit press [CTRL+C]");
                Console.ReadLine();
            }
            finally
            {
                // Ensure the channel is closed after use
                if (channel != null && channel.IsOpen)
                {
                    channel.Close();
                }
            }
        }

    }
}
