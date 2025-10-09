using CoreLib.Dtos;
using CoreLib.Entities;
using CoreLib.Interfaces;

namespace OrderService.Logic.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetAllAsync();
        Task<RestaurantDto?> GetAsync(int id);
        Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto);
        Task UpdateAsync(int id, UpdateRestaurantDto dto);
        Task DeleteAsync(int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repo;
        public RestaurantService(IRestaurantRepository repo) => _repo = repo;

        public async Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto)
        {
            var r = new Restaurant { Name = dto.Name, Address = dto.Address };
            var added = await _repo.AddAsync(r);
            return new RestaurantDto(added.Id, added.Name, added.Address);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<List<RestaurantDto>> GetAllAsync()
        {
            var list = await _repo.GetWithDishesAsync();
            return list.Select(r => new RestaurantDto(r.Id, r.Name, r.Address)).ToList();
        }

        public async Task<RestaurantDto?> GetAsync(int id)
        {
            var r = await _repo.GetAsync(id);
            if (r == null) return null;
            return new RestaurantDto(r.Id, r.Name, r.Address);
        }

        public async Task UpdateAsync(int id, UpdateRestaurantDto dto)
        {
            var r = await _repo.GetAsync(id);
            if (r == null) throw new KeyNotFoundException("Restaurant not found");
            r.Name = dto.Name;
            r.Address = dto.Address;
            await _repo.UpdateAsync(r);
        }
    }
}
