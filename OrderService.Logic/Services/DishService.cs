using CoreLib.Dtos;
using CoreLib.Entities;
using CoreLib.Interfaces;

namespace OrderService.Logic.Services
{
    public interface IDishService
    {
        Task<List<DishDto>> GetAllAsync();
        Task<DishDto?> GetAsync(int id);
        Task<DishDto> CreateAsync(CreateDishDto dto);
        Task UpdateAsync(int id, UpdateDishDto dto);
        Task DeleteAsync(int id);
    }

    public class DishService : IDishService
    {
        private readonly IDishRepository repo;
        public DishService(IDishRepository repo) => this.repo = repo;

        public async Task<DishDto> CreateAsync(CreateDishDto dto)
        {
            var d = new Dish { Name = dto.Name, Price = dto.Price, RestaurantId = dto.RestaurantId };
            var added = await repo.AddAsync(d);
            return new DishDto(added.Id, added.Name, added.Price, added.RestaurantId);
        }

        public async Task DeleteAsync(int id) => await repo.DeleteAsync(id);

        public async Task<List<DishDto>> GetAllAsync()
        {
            var list = await repo.GetAllAsync();
            return list.Select(d => new DishDto(d.Id, d.Name, d.Price, d.RestaurantId)).ToList();
        }

        public async Task<DishDto?> GetAsync(int id)
        {
            var d = await repo.GetAsync(id);
            if (d == null) return null;
            return new DishDto(d.Id, d.Name, d.Price, d.RestaurantId);
        }

        public async Task UpdateAsync(int id, UpdateDishDto dto)
        {
            var d = await repo.GetAsync(id);
            if (d == null) throw new KeyNotFoundException("Dish not found");
            d.Name = dto.Name;
            d.Price = dto.Price;
            await repo.UpdateAsync(d);
        }
    }
}
