using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DeliveryService.Infrastructure.Rabbit;

public class RabbitMqRpcClient : IDisposable
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string replyQueueName;
    private readonly EventingBasicConsumer consumer;
    private readonly IBasicProperties props;

    public RabbitMqRpcClient(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString), DispatchConsumersAsync = true };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        replyQueueName = channel.QueueDeclare(queue: "", exclusive: true).QueueName;
        consumer = new EventingBasicConsumer(channel);
        props = channel.CreateBasicProperties();
        props.ReplyTo = replyQueueName;
    }

    public Task<string> CallAsync(string queueName, string message, TimeSpan? timeout = null)
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;

        void ReceivedHandler(object? model, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            }
        }

        consumer.Received += ReceivedHandler;

        var messageBytes = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: props, body: messageBytes);

        channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

        if (timeout != null)
        {
            var ct = new CancellationTokenSource(timeout.Value);
            ct.Token.Register(() =>
            {
                tcs.TrySetException(new TimeoutException("RPC timeout"));
            });
        }

        return tcs.Task;
    }

    public void Dispose()
    {
        try { channel?.Dispose(); }
        catch { }
        try { connection?.Dispose(); }
        catch { }
    }
}