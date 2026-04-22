using System;
using System.Reflection.Metadata;
using LMS.Contracts.Abstractions;

namespace LMS.EventBus.Abstractions;

public interface IEventBusSubscriptionsManager
{
    // called by program.cs at startup
    // to register the event handlers in the DI container 
    void AddSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void RemoveSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    bool HasSubscriptionsForEvent(string eventName);

    Type GetHandlerTypeForEvent(string eventName);

    Type GetEventTypeForEvent(string eventName);

    string GetEventKey<T>() where T : IIntegrationEvent;
}
