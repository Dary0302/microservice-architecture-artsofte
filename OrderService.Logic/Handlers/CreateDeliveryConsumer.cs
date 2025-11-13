using CoreLib.Transport.Contracts;
using MassTransit;

public class CreateDeliveryConsumer : IConsumer<CreateDelivery>
{
    private readonly IPublishEndpoint _publish;

    public CreateDeliveryConsumer(IPublishEndpoint publish)
    {
        _publish = publish;
    }

    public async Task Consume(ConsumeContext<CreateDelivery> context)
    {
        Console.WriteLine($"[Delivery] Creating delivery for {context.Message.Product}");
        await _publish.Publish(new DeliveryCompleted(context.Message.CorrelationId));
    }
}