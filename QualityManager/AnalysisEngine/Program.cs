// See https://aka.ms/new-console-template for more information
using Shared.Configuration;
using Shared.Messaging;
using AnalysisEngine.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMQ"));

        services.AddSingleton<RabbitMqConnection>();

        services.AddSingleton<FoodAnalysisListener>();

        services.AddSingleton<AnalysisService>();

        services.AddHostedService<RabbitMqListenerService>();
    })
    .Build();

await builder.RunAsync();