using Microsoft.Extensions.Options;
using Shared.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.Messaging
{
    public class RabbitMqConnection
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly ConnectionFactory _connectionFactory;
        private readonly RabbitMqSettings _settings;

        public RabbitMqConnection(IOptions<RabbitMqSettings> settings)
        {
            _settings = settings.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
        }

        public async Task<bool> InitializeConnectionAsync()
        {
            int attempt = 0;
            int[] retryIntervals = { 500, 1000, 3000, 5000, 10000 };

            while (attempt < retryIntervals.Length)
            {
                try
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        Console.WriteLine($"[Attempt {attempt + 1}] Trying to connect to RabbitMQ...");
                        _connection = await _connectionFactory.CreateConnectionAsync();
                    }

                    if (_connection != null && _connection.IsOpen && (_channel == null || !_channel.IsOpen))
                    {
                        _channel = await _connection.CreateChannelAsync();
                        Console.WriteLine("Successfully connected to RabbitMQ.");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Attepmpt: {attempt}] Error connecting to RabbitMQ: {ex.Message}");
                }

                await Task.Delay(retryIntervals[attempt]);
                attempt++;
            }
            Console.WriteLine("Failed to connect to RabbitMQ after multiple attempts.");
            return false;
        }

        public async Task QueuePurgeAsync(string queue, CancellationToken cancellationToken)
        {
            await _channel.QueuePurgeAsync(queue, cancellationToken);
        }

        public async Task<QueueDeclareOk> QueueDeclareAsync(string queue, CancellationToken cancellationToken)
        {
            return await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: true, arguments: null, noWait: false, cancellationToken);
        }

        public async Task<string> BasicConsumeAsync(string queue, IAsyncBasicConsumer consumer, CancellationToken cancellationToken)
        {
            return await _channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
        }

        public async Task BasicPublishAsync(string routingKey, byte[] messageBody, CancellationToken cancellationToken)
        {
            await QueueDeclareAsync(routingKey, cancellationToken);

            BasicProperties basicProperties = new BasicProperties();
            basicProperties.ContentType = "application/json";
            basicProperties.DeliveryMode = DeliveryModes.Persistent;

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: routingKey, mandatory: false, basicProperties: basicProperties, body: messageBody, cancellationToken: cancellationToken);
        }

        public async Task BasicAckAsync(ulong tag)
        {
            await _channel.BasicAckAsync(tag, false);
        }

        public AsyncEventingBasicConsumer CreateBasicConsumer()
        {
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);

            return consumer;
        }

        public async Task CloseAsync()
        {
            if (_channel != null && _channel.IsOpen)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null && _connection.IsOpen)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}