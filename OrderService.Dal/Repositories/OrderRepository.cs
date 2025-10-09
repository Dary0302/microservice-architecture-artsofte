using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Dal.Data;

namespace OrderService.Dal.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) => _db = db;

        public async Task<Order> AddAsync(Order entity)
        {
            _db.Orders.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.Orders.FindAsync(id);
            if (e == null) return;
            _db.Orders.Remove(e);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync() => await _db.Orders.ToListAsync();

        public async Task<Order?> GetAsync(int id) => await _db.Orders.FindAsync(id);

        public async Task<Order?> GetWithItemsAsync(int id) => await _db.Orders.Include(o => o.Items).ThenInclude(i => i.Dish).FirstOrDefaultAsync(o => o.Id == id);

        public async Task<List<Order>> GetByUserAsync(int userId) => await _db.Orders.Where(o => o.UserId == userId).Include(o => o.Items).ToListAsync();

        public async Task UpdateAsync(Order entity)
        {
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
