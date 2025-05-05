using MediatR;
using Messaging.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel;
using SharedKernel.Contracts;
using System.Text;

public class UserRegisteredConsumer : BackgroundService
{
    private readonly RabbitMQConnection _connection;
    private readonly IServiceProvider _serviceProvider;

    public UserRegisteredConsumer(RabbitMQConnection connection, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateChannel();  

        channel.QueueDeclare(
            queue: "user_registered_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = System.Text.Json.JsonSerializer.Deserialize<UserRegisteredEvent>(json);

                if (message is not null)
                {
                    using var scope = _serviceProvider.CreateScope();  

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var command = new UserRegistrationDTO
                    {
                        AuthId = message.UserId,
                        Email = message.Email, 
                    };

                    await mediator.Send(command);
                }

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error consuming message: {ex.Message}");
            }
        };

        channel.BasicConsume(
            queue: "user_registered_queue",
            autoAck: false,
            consumer: consumer
        );
    }
}
