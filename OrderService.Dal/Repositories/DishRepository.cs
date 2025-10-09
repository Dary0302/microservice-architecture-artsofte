using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext db;
        public DishRepository(AppDbContext db) => this.db = db;
        public async Task<Dish> AddAsync(Dish entity)
        {
            db.Dishes.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await db.Dishes.FindAsync(id);
            if (e == null) return;
            db.Dishes.Remove(e);
            await db.SaveChangesAsync();
        }

        public async Task<List<Dish>> GetAllAsync() => await db.Dishes.ToListAsync();

        public async Task<Dish?> GetAsync(int id) => await db.Dishes.FindAsync(id);

        public async Task<List<Dish>> GetByRestaurantAsync(int restaurantId) => await db.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();

        public async Task UpdateAsync(Dish entity)
        {
            db.Dishes.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}
