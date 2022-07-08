using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CrossLang.QueueHelper;
public static class RabbitMQHelper
{
    private static IConnection connection { get; set; }

    private static ConnectionFactory factory { get; set; }

    public static IConnection getConnection()
    {
        if (connection == null)
        {
            connection = getFactory().CreateConnection();
        }
        return connection;
    }

    public static ConnectionFactory getFactory()
    {
        if (factory == null)
        {
            factory = new ConnectionFactory { HostName = "localhost" };
        }
        return factory;
    }

    public static void Enqueue<T>(string queueName, RabbitMQMessage<T> message)
    {
        using (var channel = getConnection().CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var strMessage = JsonConvert.SerializeObject(message);

            var encodedMessage = Encoding.UTF8.GetBytes(strMessage);

            channel.BasicPublish("", queueName, null, encodedMessage);
        }
    }

    public static void Subscribe<T>(string queueName, Action<RabbitMQMessage<T>> action)
    {
        using (var channel = getConnection().CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var decodedMsg = JsonConvert.DeserializeObject<RabbitMQMessage<T>>(message);

                action(decodedMsg);
            };
        }
    }
}

