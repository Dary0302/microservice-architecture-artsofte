using DeliveryService.Core.Entities;
using DeliveryService.Core.Interfaces;

namespace DeliveryService.Application.Services;

public interface ICourierService
{
    Task<List<Courier>> GetAllAsync();
    Task<Courier?> GetAsync(int id);
    Task<Courier> CreateAsync(Courier c);
    Task UpdateAsync(int id, Courier c);
    Task DeleteAsync(int id);
}

public class CourierService : ICourierService
{
    private readonly ICourierRepository repo;
    public CourierService(ICourierRepository repo) => this.repo = repo;

    public async Task<Courier> CreateAsync(Courier c) => await repo.AddAsync(c);

    public async Task DeleteAsync(int id) => await repo.DeleteAsync(id);

    public async Task<List<Courier>> GetAllAsync() => await repo.ListAsync();

    public async Task<Courier?> GetAsync(int id) => await repo.GetAsync(id);

    public async Task UpdateAsync(int id, Courier c)
    {
        var ex = await repo.GetAsync(id);
        if (ex == null) throw new KeyNotFoundException("Courier not found");
        ex.Name = c.Name;
        ex.Phone = c.Phone;
        await repo.UpdateAsync(ex);
    }
}