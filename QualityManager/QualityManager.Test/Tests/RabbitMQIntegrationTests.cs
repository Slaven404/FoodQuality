using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using QualityManager.DTOs.Requests;
using RabbitMQ.Client.Events;
using Shared.Configuration;
using Shared.Messaging;
using System.Text;

namespace QualityManager.Test.Tests
{
    public class RabbitMQIntegrationTests : IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConnection _rabbitMqConnection;
        private readonly RabbitMqSettings _rabbitMqSettings;

        public RabbitMQIntegrationTests()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                                            .SetBasePath(AppContext.BaseDirectory)
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                            .Build();

            _rabbitMqSettings = config.GetSection("RabbitMq").Get<RabbitMqSettings>();

            ServiceCollection services = new ServiceCollection();

            services.Configure<RabbitMqSettings>(config.GetSection("RabbitMq"));

            services.AddSingleton<RabbitMqConnection>();

            _serviceProvider = services.BuildServiceProvider();

            _rabbitMqConnection = _serviceProvider.GetRequiredService<RabbitMqConnection>();
        }

        [Fact]
        public async Task SendMessage_ShouldBeReceivedByAnalysisEngine()
        {
            FoodBatchRequest testMessage = new FoodBatchRequest
            {
                AnalysisTypeId = 1,
                FoodName = "Milk",
                SerialNumber = "111222333"
            };
            byte[] messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testMessage));
            string? receivedMessage = null;

            bool connection = await _rabbitMqConnection.InitializeConnectionAsync();

            if (connection)
            {
                await _rabbitMqConnection.QueueDeclareAsync(_rabbitMqSettings.FoodAnalysisRequestQueue, CancellationToken.None);
                await _rabbitMqConnection.QueuePurgeAsync(_rabbitMqSettings.FoodAnalysisRequestQueue, CancellationToken.None);

                Task<string?> consumeTask = ConsumeMessageAsync();

                await _rabbitMqConnection.BasicPublishAsync(_rabbitMqSettings.FoodAnalysisRequestQueue, messageBytes, CancellationToken.None);

                receivedMessage = await consumeTask;
            }

            Assert.NotNull(receivedMessage);
            FoodBatchRequest? receivedObject = JsonConvert.DeserializeObject<FoodBatchRequest>(receivedMessage);
            Assert.NotNull(receivedObject);
            Assert.Equal(testMessage.AnalysisTypeId, receivedObject.AnalysisTypeId);
            Assert.Equal(testMessage.FoodName, receivedObject.FoodName);
            Assert.Equal(testMessage.SerialNumber, receivedObject.SerialNumber);
        }

        private async Task<string?> ConsumeMessageAsync()
        {
            TaskCompletionSource<string?> tcs = new TaskCompletionSource<string?>();
            AsyncEventingBasicConsumer consumer = _rabbitMqConnection.CreateBasicConsumer();

            consumer.ReceivedAsync += async (model, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                tcs.TrySetResult(message);
                await Task.CompletedTask;
            };

            await _rabbitMqConnection.BasicConsumeAsync(_rabbitMqSettings.FoodAnalysisRequestQueue, consumer, CancellationToken.None);
            return await Task.WhenAny(tcs.Task, Task.Delay(5000)) == tcs.Task ? tcs.Task.Result : null;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => Task.CompletedTask;
    }
}