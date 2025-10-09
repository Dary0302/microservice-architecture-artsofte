using CoreLib.Dtos;
using CoreLib.Entities;
using CoreLib.Interfaces;

namespace OrderService.Logic.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto?> GetAsync(int id);
        Task<List<OrderDto>> GetByUserAsync(int userId);
        Task UpdateStatusAsync(int id, OrderStatus status);
        Task DeleteAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                RestaurantId = dto.RestaurantId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Created
            };

            foreach (var it in dto.Items)
            {
                order.Items.Add(new OrderItem { DishId = it.DishId, Quantity = it.Quantity });
            }

            var added = await _repo.AddAsync(order);

            var itemsDto = added.Items.Select(i => new OrderItemDto(i.DishId, i.Quantity)).ToList();
            return new OrderDto(added.Id, added.UserId, added.RestaurantId, added.CreatedAt, added.Status, itemsDto);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<OrderDto?> GetAsync(int id)
        {
            var o = await _repo.GetWithItemsAsync(id);
            if (o == null) return null;
            var items = o.Items.Select(i => new OrderItemDto(i.DishId, i.Quantity)).ToList();
            return new OrderDto(o.Id, o.UserId, o.RestaurantId, o.CreatedAt, o.Status, items);
        }

        public async Task<List<OrderDto>> GetByUserAsync(int userId)
        {
            var list = await _repo.GetByUserAsync(userId);
            return list.Select(o => new OrderDto(o.Id, o.UserId, o.RestaurantId, o.CreatedAt, o.Status, o.Items.Select(i => new OrderItemDto(i.DishId, i.Quantity)).ToList())).ToList();
        }

        public async Task UpdateStatusAsync(int id, OrderStatus status)
        {
            var o = await _repo.GetAsync(id);
            if (o == null) throw new KeyNotFoundException("Order not found");
            o.Status = status;
            await _repo.UpdateAsync(o);
        }
    }
}
