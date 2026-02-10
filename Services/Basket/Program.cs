using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Basket.Validators;
using Basket.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add validation services (FluentValidation + MediatR pipeline + ApiBehavior customization)
builder.Services.AddValidationServices();

// Redis
builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
{
    var logger = sp.GetService<Microsoft.Extensions.Logging.ILoggerFactory>()?.CreateLogger("Redis");
    var connStr = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

    // parse configuration and allow multiplexer to keep retrying instead of throwing on first connect
    var options = StackExchange.Redis.ConfigurationOptions.Parse(connStr);
    options.AbortOnConnectFail = false; // continue retrying
    options.ConnectRetry = 5;
    options.ConnectTimeout = 10000;
    options.SyncTimeout = 10000;
    options.KeepAlive = 180;

    try
    {
        logger?.LogInformation("Connecting to Redis at {connStr}", connStr);
        return StackExchange.Redis.ConnectionMultiplexer.Connect(options);
    }
    catch (Exception ex)
    {
        logger?.LogWarning(ex, "Failed to connect to Redis at {connStr}. Will try localhost:6379 as fallback.");

        try
        {
            var fallback = StackExchange.Redis.ConfigurationOptions.Parse("localhost:6379");
            fallback.AbortOnConnectFail = false;
            fallback.ConnectRetry = 5;
            fallback.ConnectTimeout = 10000;
            fallback.SyncTimeout = 10000;
            fallback.KeepAlive = 180;
            logger?.LogInformation("Attempting Redis fallback to localhost:6379");
            return StackExchange.Redis.ConnectionMultiplexer.Connect(fallback);
        }
        catch (Exception ex2)
        {
            logger?.LogError(ex2, "Failed to connect to Redis fallback localhost:6379");
            // rethrow so app start can fail if you prefer
            throw;
        }
    }
});

// Repositories
builder.Services.AddScoped<Basket.Repositories.IBasketRepository, Basket.Repositories.BasketRepository>();

// gRPC Clients
builder.Services.AddScoped<Basket.Services.IDiscountGrpcService, Basket.Services.DiscountGrpcService>();

// MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// MassTransit RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ") ?? "rabbitmq://guest:guest@localhost");
    });
});
// MediatR pipeline behaviors
//builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(Basket.Behaviors.ValidationBehavior<,>));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use validation exception middleware so FluentValidation exceptions are returned in same shape
//app.UseMiddleware<Basket.Middleware.ValidationExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
