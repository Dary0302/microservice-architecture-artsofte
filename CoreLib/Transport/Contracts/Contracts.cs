namespace CoreLib.Transport.Contracts;

public record OrderCreated(Guid OrderId, string Product);
public record DeliveryCreated(Guid DeliveryId, Guid OrderId);
public record OrderSubmitted(Guid CorrelationId, string Product);
public record CreateDelivery(Guid CorrelationId, string Product);
public record DeliveryCompleted(Guid CorrelationId);