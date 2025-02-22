using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using QualityManager.DTOs.Responses;
using QualityManager.DTOs.Requests;
using Shared.Messaging;
using Shared.Configuration;

namespace QualityManager.Services
{
    public class AnalysisListener
    {
        private readonly RabbitMqSettings _settings;
        private readonly RabbitMqConnection _rabbitMqConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AnalysisListener(IOptions<RabbitMqSettings> settings, RabbitMqConnection rabbitMqConnection, IServiceScopeFactory serviceScopeFactory)
        {
            _settings = settings.Value;
            _rabbitMqConnection = rabbitMqConnection;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            await _rabbitMqConnection.QueueDeclareAsync(_settings.FoodAnalysisResponseQueue, cancellationToken);
            FoodAnalysisProcessResponse response = new FoodAnalysisProcessResponse();
            AsyncEventingBasicConsumer consumer = _rabbitMqConnection.CreateBasicConsumer();
            consumer.ReceivedAsync += async (sender, e) =>
            {
                string messageBody = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Received message: {messageBody}");
                response = JsonSerializer.Deserialize<FoodAnalysisProcessResponse>(messageBody) ?? new FoodAnalysisProcessResponse();

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var servc = scope.ServiceProvider.CreateScope();

                    IFoodAnalysisService analysisService = scope.ServiceProvider.GetRequiredService<IFoodAnalysisService>();
                    await analysisService.UpdateFoodProcessStatus(response);
                    Console.WriteLine($"Analysis {response.SerialNumber}: Status updated");
                }

                await _rabbitMqConnection.BasicAckAsync(e.DeliveryTag);
            };

            await _rabbitMqConnection.BasicConsumeAsync(_settings.FoodAnalysisResponseQueue, consumer, cancellationToken);
        }

        public async Task SendFoodBatchToRabbitMqAsync(FoodBatchRequest analysisResult, CancellationToken cancellationToken)
        {
            byte[] messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(analysisResult));

            await _rabbitMqConnection.BasicPublishAsync(_settings.FoodAnalysisRequestQueue, messageBody, cancellationToken);

            Console.WriteLine("Analysis result sent to RabbitMQ request queue.");
        }
    }
}