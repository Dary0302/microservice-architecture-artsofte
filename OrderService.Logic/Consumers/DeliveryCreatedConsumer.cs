using CoreLib.Transport.Contracts;
using MassTransit;

public class DeliveryCreatedConsumer : IConsumer<DeliveryCreated>
{
    public Task Consume(ConsumeContext<DeliveryCreated> context)
    {
        Console.WriteLine($"[OrderService] Delivery confirmed for order {context.Message.OrderId}");
        return Task.CompletedTask;
    }
}