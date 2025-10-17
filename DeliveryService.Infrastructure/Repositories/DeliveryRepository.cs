using Microsoft.EntityFrameworkCore;
using DeliveryService.Core.Entities;
using DeliveryService.Core.Interfaces;
using DeliveryService.Infrastructure.Persistence;

namespace DeliveryService.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly DeliveryDbContext db;
    public DeliveryRepository(DeliveryDbContext db) => this.db = db;

    public async Task<Delivery> AddAsync(Delivery d)
    {
        db.Deliveries.Add(d);
        await db.SaveChangesAsync();
        return d;
    }

    public async Task<Delivery?> GetAsync(int id) => await db.Deliveries.FindAsync(id);

    public async Task<List<Delivery>> ListByOrderAsync(int orderId) => await db.Deliveries.Where(d => d.OrderId == orderId).ToListAsync();

    public async Task UpdateAsync(Delivery d)
    {
        db.Deliveries.Update(d);
        await db.SaveChangesAsync();
    }
}