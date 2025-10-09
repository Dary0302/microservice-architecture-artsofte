using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext db;
        public RestaurantRepository(AppDbContext db) => this.db = db;
        public async Task<Restaurant> AddAsync(Restaurant entity)
        {
            db.Restaurants.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await db.Restaurants.FindAsync(id);
            if (e == null) return;
            db.Restaurants.Remove(e);
            await db.SaveChangesAsync();
        }

        public async Task<List<Restaurant>> GetAllAsync() => await db.Restaurants.ToListAsync();

        public async Task<Restaurant?> GetAsync(int id) => await db.Restaurants.FindAsync(id);

        public async Task<List<Restaurant>> GetWithDishesAsync() => await db.Restaurants.Include(r => r.Dishes).ToListAsync();

        public async Task UpdateAsync(Restaurant entity)
        {
            db.Restaurants.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}
