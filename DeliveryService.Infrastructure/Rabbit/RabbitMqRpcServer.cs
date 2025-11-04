using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CoreLib.Transport;
using System.Text.Json;

namespace DeliveryService.Infrastructure.Rabbit;

public class RabbitMqRpcServer : IDisposable
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string queueName;
    private readonly IRpcHandler handler;

    public RabbitMqRpcServer(string connectionString, string queueName, IRpcHandler handler)
    {
        this.handler = handler;
        this.queueName = queueName;
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            string response;
            try
            {
                var message = Encoding.UTF8.GetString(body);
                var endpoint = props.Headers != null && props.Headers.TryGetValue("endpoint", out var value)
                    ? Encoding.UTF8.GetString((byte[])value)
                    : "default";

                response = await handler.HandleAsync(endpoint, message);
            }
            catch (Exception ex)
            {
                response = JsonSerializer.Serialize(new { error = ex.Message });
            }

            var responseBytes = Encoding.UTF8.GetBytes(response);
            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps,
                body: responseBytes);
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        try { channel?.Dispose(); }
        catch { }
        try { connection?.Dispose(); }
        catch { }
    }
}