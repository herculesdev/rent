using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Rent.Shared.Library.Messaging;

public class RabbitMqMessageDispatcher : IMessageDispatcher
{
    private ConnectionFactory _factory;
    
    public RabbitMqMessageDispatcher(IOptions<RabbitMqConfig> configOptions)
    {
        var config = configOptions.Value;
        _factory = new ConnectionFactory();
        _factory.HostName = config.Hostname;
        _factory.Port = config.Port;
        _factory.UserName = config.Username;
        _factory.Password = config.Password;
        _factory.VirtualHost = config.VirtualHost;
        _factory.AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled;
        _factory.TopologyRecoveryEnabled = config.TopologyRecoveryEnabled;
        _factory.RequestedConnectionTimeout = TimeSpan.FromMilliseconds(config.RequestedConnectionTimeout);
        _factory.RequestedHeartbeat = TimeSpan.FromSeconds(config.RequestedHeartbeat);
        _factory.ClientProvidedName = config.ClientProvidedName;    
    }

    private void Publish<TMessage>(string queueOrStreamName, TMessage message, bool publishToStream)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();
        var queueDeclareArgs = new Dictionary<string, object>();
        
        if(publishToStream)
            queueDeclareArgs.Add("x-queue-type", "stream");
        
        channel.QueueDeclare(queueOrStreamName, durable: true, exclusive: false, autoDelete: false, arguments: queueDeclareArgs);
        
        var messageBodyBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(exchange: "", queueOrStreamName, null, messageBodyBytes);
        
        channel.Close();
        connection.Close();
    }
    
    public void PublishToStream<TMessage>(string streamName , TMessage message)
    {
        Publish(streamName, message, publishToStream: true);
    }
    
    public void  Publish<TMessage>(string streamName , TMessage message)
    {
        Publish(streamName, message, publishToStream: false);
    }
}