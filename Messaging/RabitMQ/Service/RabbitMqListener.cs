using Messaging.RabitMQ.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Messaging.RabitMQ.Service
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IMessageSubscriber _subscriber;

        public RabbitMqListener(IMessageSubscriber subscriber)
        {
            _subscriber = subscriber;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriber.SubscribeToQueue("emri-i-queues", message =>
            {
                Console.WriteLine($"U pranua mesazhi: {message}"); 
            });

            return Task.CompletedTask;
        }
    }
}
