using MediatR;
using Discount.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<Discount.Repositories.IDiscountRepository, Discount.Repositories.DiscountRepository>();

// MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// gRPC
builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<DiscountGrpcService>();

app.Run();
