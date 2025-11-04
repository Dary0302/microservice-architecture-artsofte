using MassTransit;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string? Product { get; set; }
    public string CurrentState { get; set; } = "";
}