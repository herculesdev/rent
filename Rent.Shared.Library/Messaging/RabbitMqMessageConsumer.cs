using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rent.Shared.Library.Messaging;

public class RabbitMqMessageConsumer : IMessageConsumer
{
    private ConnectionFactory _factory;
    private ILogger _logger;
    
    public RabbitMqMessageConsumer(IOptions<RabbitMqConfig> configOptions, ILogger<RabbitMqMessageConsumer> logger)
    {
        var config = configOptions.Value;
        _factory = new ConnectionFactory();
        _factory.HostName = config.Hostname;
        _factory.Port = config.Port;
        _factory.UserName = config.Username;
        _factory.Password = config.Password;
        _factory.VirtualHost = config.VirtualHost;
        _factory.ClientProvidedName = config.ClientProvidedName;

        _logger = logger;
    }
    public void ConsumeFromStream(string queueOrStreamName, object? lastProcessedOffset, EventHandler<BasicDeliverEventArgs> eventHandler)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();
        var queueDeclareArgs = new Dictionary<string, object> { { "x-queue-type", "stream" } };
        object nextOffset = lastProcessedOffset != null ? ((long)lastProcessedOffset) + 1 : "first";
        var queueConsumeArgs = new Dictionary<string, object> { { "x-stream-offset",  nextOffset} };
        
        channel.QueueDeclare(queueOrStreamName, durable: true, exclusive: false, autoDelete: false, arguments: queueDeclareArgs);
        channel.BasicQos(0, 100, false);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += eventHandler;
        
        channel.BasicConsume(queueOrStreamName, false, "", queueConsumeArgs, consumer);
        
        //channel.Close();
        //connection.Close();
    }
}