namespace DeliveryService.Core.Entities;

public class DeliveryLog
{
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = null!;
}