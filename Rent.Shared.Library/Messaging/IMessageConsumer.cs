using RabbitMQ.Client.Events;

namespace Rent.Shared.Library.Messaging;

public interface IMessageConsumer
{
    void ConsumeFromStream(string queueOrStreamName, object offset, EventHandler<BasicDeliverEventArgs> eventHandler);
}