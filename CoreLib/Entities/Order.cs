namespace CoreLib.Entities
{
    public enum OrderStatus { Created, Confirmed, Preparing, OnTheWay, Delivered, Cancelled }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
