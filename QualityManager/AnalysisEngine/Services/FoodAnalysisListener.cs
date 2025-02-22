using AnalysisEngine.DTOs;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using Shared.Configuration;
using Shared.Messaging;
using System.Text;
using System.Text.Json;

namespace AnalysisEngine.Services
{
    public class FoodAnalysisListener
    {
        private readonly AnalysisService _analysisService;
        private readonly RabbitMqSettings _settings;
        private readonly RabbitMqConnection _rabbitMqConnection;

        public FoodAnalysisListener(IOptions<RabbitMqSettings> settings, AnalysisService analysisService, RabbitMqConnection rabbitMqConnection)
        {
            _settings = settings.Value;
            _analysisService = analysisService;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            await _rabbitMqConnection.QueueDeclareAsync(_settings.FoodAnalysisRequestQueue, cancellationToken);

            AsyncEventingBasicConsumer consumer = _rabbitMqConnection.CreateBasicConsumer();

            consumer.ReceivedAsync += async (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Received message: {message}");
                AnalysisRequestDto request = JsonSerializer.Deserialize<AnalysisRequestDto>(message) ?? new AnalysisRequestDto();

                AnalysisResultDto analysisResult = new AnalysisResultDto();
                analysisResult.SerialNumber = request.SerialNumber;

                await SendAnalysisResultToRabbitMqAsync(analysisResult, cancellationToken);

                analysisResult.Result = await _analysisService.PerformAnalysisAsync(request);

                await SendAnalysisResultToRabbitMqAsync(analysisResult, cancellationToken);
                await _rabbitMqConnection.BasicAckAsync(e.DeliveryTag);
            };

            await _rabbitMqConnection.BasicConsumeAsync(_settings.FoodAnalysisRequestQueue, consumer, cancellationToken);
        }

        public async Task SendAnalysisResultToRabbitMqAsync(AnalysisResultDto analysisResult, CancellationToken cancellationToken)
        {
            byte[] messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(analysisResult));

            await _rabbitMqConnection.BasicPublishAsync(_settings.FoodAnalysisResponseQueue, messageBody, cancellationToken);

            Console.WriteLine("Analysis result sent to RabbitMQ response queue.");
        }
    }
}