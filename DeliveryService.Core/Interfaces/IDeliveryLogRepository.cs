using DeliveryService.Core.Entities;

namespace DeliveryService.Core.Interfaces;

public interface IDeliveryLogRepository
{
    Task<List<DeliveryLog>> ListByDeliveryAsync(int deliveryId);
    Task<DeliveryLog> AddAsync(DeliveryLog log);
}