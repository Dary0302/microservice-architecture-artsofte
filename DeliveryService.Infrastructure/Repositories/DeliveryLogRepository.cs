using Microsoft.EntityFrameworkCore;
using DeliveryService.Core.Entities;
using DeliveryService.Core.Interfaces;
using DeliveryService.Infrastructure.Persistence;

namespace DeliveryService.Infrastructure.Repositories;

public class DeliveryLogRepository : IDeliveryLogRepository
{
    private readonly DeliveryDbContext db;
    public DeliveryLogRepository(DeliveryDbContext db) => this.db = db;

    public async Task<DeliveryLog> AddAsync(DeliveryLog log)
    {
        db.DeliveryLogs.Add(log);
        await db.SaveChangesAsync();
        return log;
    }

    public async Task<List<DeliveryLog>> ListByDeliveryAsync(int deliveryId) => await db.DeliveryLogs.Where(l => l.DeliveryId == deliveryId).ToListAsync();
}