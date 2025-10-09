using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Entities;

namespace CoreLib.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<List<Restaurant>> GetWithDishesAsync();
    }

    public interface IDishRepository : IRepository<Dish>
    {
        Task<List<Dish>> GetByRestaurantAsync(int restaurantId);
    }

    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(int id);
        Task<List<Order>> GetByUserAsync(int userId);
    }
}
