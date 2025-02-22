using AnalysisEngine.Services;
using Microsoft.EntityFrameworkCore;
using QualityManager.Data;
using QualityManager.Mappings;
using QualityManager.Middleware;
using QualityManager.Repository;
using QualityManager.Resources;
using QualityManager.Services;
using Shared.Configuration;
using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(opt =>
    {
        opt.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            if (type == typeof(ValidationMessages))
            {
                return factory.Create(typeof(ValidationMessages));
            }
            else
            {
                return factory.Create(typeof(ValidationMessages));
            }
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalization();
builder.Services.AddAutoMapper(typeof(FoodAnalysisProfile));

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<AnalysisListener>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITresholdRepository, TresholdRepository>();
builder.Services.AddScoped<IFoodAnalysisRepository, FoodAnalysisRepository>();
builder.Services.AddScoped<IFoodAnalysisService, FoodAnalysisService>();

builder.Services.AddHostedService<RabbitMqListenerService>();

builder.Logging.AddConsole();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error applying database migrations.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Content("<h1>Quality Manager API is running!</h1>", "text/html"));

app.Run();