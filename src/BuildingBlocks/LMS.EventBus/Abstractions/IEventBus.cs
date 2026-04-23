using System;
using LMS.Contracts.Abstractions;

namespace LMS.EventBus.Abstractions;

//standard interface for event bus. it will be implemented by the actual event bus (e.g. Kafka, RabbitMQ, Azure Service Bus, etc.)
public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : IIntegrationEvent; //publish the event to bus
}
