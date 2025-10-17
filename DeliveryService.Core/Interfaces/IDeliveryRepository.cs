using DeliveryService.Core.Entities;

namespace DeliveryService.Core.Interfaces;

public interface IDeliveryRepository
{
    Task<Delivery?> GetAsync(int id);
    Task<List<Delivery>> ListByOrderAsync(int orderId);
    Task<Delivery> AddAsync(Delivery d);
    Task UpdateAsync(Delivery d);
}