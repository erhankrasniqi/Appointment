using Messaging.RabbitMQ.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MessageReceiver
{
    private readonly RabbitMQConnection _rabbitMqConnection;

    public MessageReceiver(RabbitMQConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public void ReceiveMessage(string queueName)
    {
        var channel = _rabbitMqConnection.CreateChannel();  

        try
        { 
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);

                Console.WriteLine($"Message received: {message}");
            };

            // Filloni konsumimin e mesazheve
            channel.BasicConsume(queueName, autoAck: true, consumer: consumer);

            Console.WriteLine($"Waiting for messages in {queueName}. To exit press [CTRL+C]");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving message: {ex.Message}");
        }
        finally
        {
            // Sigurohu që kanali është mbyllur pas përdorimit
            if (channel != null && channel.IsOpen)
            {
                channel.Close();
            }
        }
    }
}
