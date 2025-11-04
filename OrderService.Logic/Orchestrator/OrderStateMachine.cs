using CoreLib.Transport.Contracts;
using MassTransit;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; } = null!;
    public State Completed { get; private set; } = null!;

    public Event<OrderSubmitted> OrderSubmitted { get; private set; } = null!;
    public Event<DeliveryCompleted> DeliveryCompleted { get; private set; } = null!;

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderSubmitted, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
        Event(() => DeliveryCompleted, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

        Initially(
            When(OrderSubmitted)
                .Then(ctx =>
                {
                    ctx.Instance.Product = ctx.Data.Product;
                    Console.WriteLine($"[Saga] Order submitted: {ctx.Data.Product}");
                })
                .Send(new Uri("queue:create-delivery"), ctx => new CreateDelivery(ctx.Instance.CorrelationId, ctx.Instance.Product!))
                .TransitionTo(Submitted)
        );

        During(Submitted,
            When(DeliveryCompleted)
                .Then(ctx => Console.WriteLine($"[Saga] Delivery completed for {ctx.Instance.Product}"))
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}