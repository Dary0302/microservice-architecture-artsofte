using CoreLib.Transport.Contracts;
using MassTransit;

public class OrderCreatedConsumer(IPublishEndpoint publishEndpoint) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        Console.WriteLine($"[DeliveryService] Received order: {context.Message.OrderId}");
        var deliveryId = Guid.NewGuid();

        await publishEndpoint.Publish(new DeliveryCreated(deliveryId, context.Message.OrderId));
    }
}