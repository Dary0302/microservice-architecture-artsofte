using DeliveryService.Core.Entities;
using DeliveryService.Core.Interfaces;

namespace DeliveryService.Application.Services;

public interface IDeliveryServiceApp
{
    Task<Delivery> AssignAsync(int orderId, int courierId);
    Task<Delivery?> GetAsync(int id);
    Task UpdateStatusAsync(int id, DeliveryStatus status);
}

public class DeliveryServiceApp : IDeliveryServiceApp
{
    private readonly IDeliveryRepository repo;
    private readonly ICourierRepository courierRepo;
    private readonly IDeliveryLogRepository logRepo;

    public DeliveryServiceApp(IDeliveryRepository repo, ICourierRepository courierRepo, IDeliveryLogRepository logRepo)
    {
        this.repo = repo;
        this.courierRepo = courierRepo;
        this.logRepo = logRepo;
    }

    public async Task<Delivery> AssignAsync(int orderId, int courierId)
    {
        var c = await courierRepo.GetAsync(courierId);
        if (c == null) throw new KeyNotFoundException("Courier not found");

        var d = new Delivery { OrderId = orderId, CourierId = courierId, AssignedAt = DateTime.UtcNow, Status = DeliveryStatus.Assigned };
        var added = await repo.AddAsync(d);

        await logRepo.AddAsync(new DeliveryLog { DeliveryId = added.Id, Timestamp = DateTime.UtcNow, Message = "Assigned" });
        return added;
    }

    public async Task<Delivery?> GetAsync(int id) => await repo.GetAsync(id);

    public async Task UpdateStatusAsync(int id, DeliveryStatus status)
    {
        var d = await repo.GetAsync(id) ?? throw new KeyNotFoundException("Delivery not found");
        d.Status = status;
        await repo.UpdateAsync(d);
        await logRepo.AddAsync(new DeliveryLog { DeliveryId = d.Id, Timestamp = DateTime.UtcNow, Message = $"Status:{status}" });
    }
}