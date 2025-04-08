using Messaging.RabbitMQ.Settings;
using Messaging.RabitMQ.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedKernel.Contracts;
using System.Text.Json;
using System.Text;

public class UserRegisteredPublisher : IUserRegisteredPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMqSettings _settings;

    public UserRegisteredPublisher(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;

        // Krijo lidhjen dhe kanal për RabbitMQ
        var factory = new ConnectionFactory() { HostName = _settings.HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Sigurohemi që të krijojmë një exchange dhe një queue për ngjarjet.
        _channel.ExchangeDeclare(exchange: "user_exchange", type: ExchangeType.Fanout);
        _channel.QueueDeclare(queue: "user_registered_queue", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind("user_registered_queue", "user_exchange", "");
    }

    public async Task PublishUserRegisteredEventAsync(UserRegisteredEvent userRegisteredEvent)
    {
        var message = JsonSerializer.Serialize(userRegisteredEvent);
        var body = Encoding.UTF8.GetBytes(message);

        // Dërgo mesazhin në RabbitMQ
        _channel.BasicPublish(exchange: "user_exchange", routingKey: "", basicProperties: null, body: body);

        await Task.CompletedTask;
    }

    // Sigurohemi që të mbyllim kanalin dhe lidhjen kur të mbarojmë
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
