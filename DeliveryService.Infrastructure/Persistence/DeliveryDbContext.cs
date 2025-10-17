using Microsoft.EntityFrameworkCore;
using DeliveryService.Core.Entities;

namespace DeliveryService.Infrastructure.Persistence;

public class DeliveryDbContext : DbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> opts) : base(opts) { }

    public DbSet<Courier> Couriers { get; set; } = null!;
    public DbSet<Delivery> Deliveries { get; set; } = null!;
    public DbSet<DeliveryLog> DeliveryLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        mb.Entity<Delivery>().HasKey(d => d.Id);
    }
}