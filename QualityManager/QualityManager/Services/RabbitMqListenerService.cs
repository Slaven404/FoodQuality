﻿using QualityManager.Services;
using Shared.Messaging;

namespace AnalysisEngine.Services
{
    public class RabbitMqListenerService : BackgroundService
    {
        private readonly RabbitMqConnection _rabbitMqConnection;
        private readonly AnalysisListener _foodAnalysisListener;

        public RabbitMqListenerService(RabbitMqConnection rabbitMqConnection, AnalysisListener foodAnalysisListener)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _foodAnalysisListener = foodAnalysisListener;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("QualityManager is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                bool connected = await _rabbitMqConnection.InitializeConnectionAsync();
                if (connected)
                {
                    Console.WriteLine("RabbitMQ Listener is starting...");
                    await _foodAnalysisListener.StartListeningAsync(stoppingToken);
                    break;
                }

                Console.WriteLine("Failed to connect to RabbitMQ. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _rabbitMqConnection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}