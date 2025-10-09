using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _db;
        public RestaurantRepository(AppDbContext db) => _db = db;
        public async Task<Restaurant> AddAsync(Restaurant entity)
        {
            _db.Restaurants.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.Restaurants.FindAsync(id);
            if (e == null) return;
            _db.Restaurants.Remove(e);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Restaurant>> GetAllAsync() => await _db.Restaurants.ToListAsync();

        public async Task<Restaurant?> GetAsync(int id) => await _db.Restaurants.FindAsync(id);

        public async Task<List<Restaurant>> GetWithDishesAsync() => await _db.Restaurants.Include(r => r.Dishes).ToListAsync();

        public async Task UpdateAsync(Restaurant entity)
        {
            _db.Restaurants.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
