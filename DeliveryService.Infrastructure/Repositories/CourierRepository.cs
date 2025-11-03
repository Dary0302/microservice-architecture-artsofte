using Microsoft.EntityFrameworkCore;
using DeliveryService.Core.Entities;
using DeliveryService.Core.Interfaces;
using DeliveryService.Infrastructure.Persistence;

namespace DeliveryService.Infrastructure.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly DeliveryDbContext db;
    public CourierRepository(DeliveryDbContext db) => this.db = db;

    public async Task<Courier> AddAsync(Courier c)
    {
        db.Couriers.Add(c);
        await db.SaveChangesAsync();
        return c;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await db.Couriers.FindAsync(id);
        if (e == null) return;
        db.Couriers.Remove(e);
        await db.SaveChangesAsync();
    }

    public async Task<Courier?> GetAsync(int id) => await db.Couriers.FindAsync(id);

    public async Task<List<Courier>> ListAsync() => await db.Couriers.ToListAsync();

    public async Task UpdateAsync(Courier c)
    {
        db.Couriers.Update(c);
        await db.SaveChangesAsync();
    }
}