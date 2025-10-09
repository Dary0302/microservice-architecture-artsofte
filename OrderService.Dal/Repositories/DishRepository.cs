using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _db;
        public DishRepository(AppDbContext db) => _db = db;
        public async Task<Dish> AddAsync(Dish entity)
        {
            _db.Dishes.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.Dishes.FindAsync(id);
            if (e == null) return;
            _db.Dishes.Remove(e);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Dish>> GetAllAsync() => await _db.Dishes.ToListAsync();

        public async Task<Dish?> GetAsync(int id) => await _db.Dishes.FindAsync(id);

        public async Task<List<Dish>> GetByRestaurantAsync(int restaurantId) => await _db.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();

        public async Task UpdateAsync(Dish entity)
        {
            _db.Dishes.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
