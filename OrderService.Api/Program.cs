using CoreLib.Http;
using Microsoft.EntityFrameworkCore;
using CoreLib.Interfaces;
using CoreLib.Transport;
using MassTransit;
using OrderService.Dal.Data;
using OrderService.Dal.Repositories;
using OrderService.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<TraceIdHandler>();
builder.Services.AddHttpClient<HttpService>()
    .AddHttpMessageHandler<TraceIdHandler>();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrderService, OrderService.Logic.Services.OrderService>();

var transport = builder.Configuration["Transport"] ?? "Http";

builder.Services.AddSingleton<IRpcHandler, DeliveryService.Application.Consumers.AssignDeliveryConsumer>();

if (transport == "RabbitMq")
{
    var rabbitConn = builder.Configuration["RabbitMq:Connection"] ?? "amqp://guest:guest@localhost:5672/";
    var rpcQueue = builder.Configuration["RabbitMq:Queue"] ?? "rpc_queue";

    builder.Services.AddSingleton<ITransportService>(_ =>
        new DeliveryService.Infrastructure.Rabbit.RabbitMqTransportServiceAdapter(rabbitConn, rpcQueue));

    builder.Services.AddSingleton(sp =>
    {
        var handler = sp.GetRequiredService<IRpcHandler>();
        var server = new DeliveryService.Infrastructure.Rabbit.RabbitMqRpcServer(rabbitConn, rpcQueue, handler);
        server.Start();
        return server;
    });
}
else
{
    builder.Services.AddHttpClient<ITransportService, HttpTransportService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["DeliveryService:BaseUrl"] ?? "http://localhost:5001/");
    });
}

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
    
    x.AddConsumer<OrderCreatedConsumer>();
});

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Restaurants.Any())
    {
        var r1 = new CoreLib.Entities.Restaurant { Name = "Pizza House", Address = "Main St" };
        var r2 = new CoreLib.Entities.Restaurant { Name = "Sushi Star", Address = "Sea Ave" };
        db.Restaurants.AddRange(r1, r2);
        db.Dishes.AddRange(new CoreLib.Entities.Dish { Name = "Margarita", Price = 450, Restaurant = r1 },
            new CoreLib.Entities.Dish { Name = "Pepperoni", Price = 500, Restaurant = r1 },
            new CoreLib.Entities.Dish { Name = "Sushi set", Price = 1200, Restaurant = r2 });
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseTraceId();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();