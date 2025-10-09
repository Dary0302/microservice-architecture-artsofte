using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext db;
        public OrderRepository(AppDbContext db) => this.db = db;

        public async Task<Order> AddAsync(Order entity)
        {
            db.Orders.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await db.Orders.FindAsync(id);
            if (e == null) return;
            db.Orders.Remove(e);
            await db.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync() => await db.Orders.ToListAsync();

        public async Task<Order?> GetAsync(int id) => await db.Orders.FindAsync(id);

        public async Task<Order?> GetWithItemsAsync(int id) => await db.Orders.Include(o => o.Items).ThenInclude(i => i.Dish).FirstOrDefaultAsync(o => o.Id == id);

        public async Task<List<Order>> GetByUserAsync(int userId) => await db.Orders.Where(o => o.UserId == userId).Include(o => o.Items).ToListAsync();

        public async Task UpdateAsync(Order entity)
        {
            db.Orders.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}
