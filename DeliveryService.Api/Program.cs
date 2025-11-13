using CoreLib.Http;
using CoreLib.Transport;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Infrastructure.Persistence;
using DeliveryService.Core.Interfaces;
using DeliveryService.Infrastructure.Repositories;
using DeliveryService.Application.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DeliveryDbContext>(opt => opt.UseNpgsql(conn));
builder.Services.AddHttpClient<ITransportService, HttpTransportService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DeliveryService:BaseUrl"] ?? "http://localhost:5001/");
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
    x.AddConsumer<DeliveryCreatedConsumer>();
});

builder.Services.AddMassTransitHostedService();

// repositories
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryLogRepository, DeliveryLogRepository>();

// application services
builder.Services.AddScoped<ICourierService, CourierService>();
builder.Services.AddScoped<IDeliveryServiceApp, DeliveryServiceApp>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseTraceId();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();