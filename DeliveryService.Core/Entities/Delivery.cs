namespace DeliveryService.Core.Entities;

public enum DeliveryStatus { Pending, Assigned, InTransit, Delivered, Failed }

public class Delivery
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int CourierId { get; set; }
    public DateTime AssignedAt { get; set; }
    public DeliveryStatus Status { get; set; }
}