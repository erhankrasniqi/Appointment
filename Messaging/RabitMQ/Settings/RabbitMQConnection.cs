using RabbitMQ.Client;
using System;

namespace Messaging.RabbitMQ.Settings
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly IConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMQConnection(string hostName, string userName, string password)
        {
            _factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            };
        }

        public IModel CreateChannel()
        {
            // Kontrollo nëse lidhja është e hapur
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
            }

            // Krijo dhe kthe kanalin
            return _connection.CreateModel();
        }

        public void CloseConnection()
        {
            // Sigurohu që të mbyllësh lidhjen dhe kanalin për të shmangur leaks
            _connection?.Close();
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
