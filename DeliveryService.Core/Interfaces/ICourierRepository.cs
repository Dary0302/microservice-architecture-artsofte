using DeliveryService.Core.Entities;

namespace DeliveryService.Core.Interfaces;

public interface ICourierRepository
{
    Task<List<Courier>> ListAsync();
    Task<Courier?> GetAsync(int id);
    Task<Courier> AddAsync(Courier c);
    Task UpdateAsync(Courier c);
    Task DeleteAsync(int id);
}