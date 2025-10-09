using Microsoft.EntityFrameworkCore;
using CoreLib.Entities;

namespace OrderService.Dal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<Dish> Dishes { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Restaurant>().HasMany(r => r.Dishes).WithOne(d => d.Restaurant!).HasForeignKey(d => d.RestaurantId);
            modelBuilder.Entity<Order>().HasMany(o => o.Items).WithOne(i => i.Order!).HasForeignKey(i => i.OrderId);
        }
    }
}
