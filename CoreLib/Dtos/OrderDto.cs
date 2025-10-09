using System;
using System.Collections.Generic;
using CoreLib.Entities;

namespace CoreLib.Dtos
{
    public record OrderItemDto(int DishId, int Quantity);
    public record CreateOrderDto(int UserId, int RestaurantId, List<OrderItemDto> Items);
    public record OrderDto(int Id, int UserId, int RestaurantId, DateTime CreatedAt, OrderStatus Status, List<OrderItemDto> Items);
}
