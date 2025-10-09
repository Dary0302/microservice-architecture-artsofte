using Microsoft.EntityFrameworkCore;
using CoreLib.Interfaces;
using OrderService.Dal.Data;
using OrderService.Dal.Repositories;
using OrderService.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrderService, OrderService.Logic.Services.OrderService>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();