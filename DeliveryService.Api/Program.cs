using CoreLib.Http;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Infrastructure.Persistence;
using DeliveryService.Core.Interfaces;
using DeliveryService.Infrastructure.Repositories;
using DeliveryService.Application.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DeliveryDbContext>(opt => opt.UseNpgsql(conn));

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