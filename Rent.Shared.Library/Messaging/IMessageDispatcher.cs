namespace Rent.Shared.Library.Messaging;

public interface IMessageDispatcher
{
    void PublishToStream<TMessage>(string exchangeName, TMessage message);
    void Publish<TMessage>(string exchangeName, TMessage message);
}