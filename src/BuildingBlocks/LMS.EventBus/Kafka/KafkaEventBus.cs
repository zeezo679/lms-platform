using System;
using LMS.Contracts.Abstractions;
using LMS.EventBus.Abstractions;

namespace LMS.EventBus.Kafka;

public class KafkaEventBus : IEventBus
{
    public Task PublishAsync<T>(T @event) where T : IIntegrationEvent
    {
        throw new NotImplementedException();
    }

    public Task SubscribeAsync<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public Task UnsubscribeAsync<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }
}
