using System;
using LMS.Contracts.Abstractions;
using LMS.EventBus.Abstractions;

namespace LMS.EventBus.Kafka;

public class InMemorySubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, Type> _handlers = new();
    private readonly Dictionary<string, Type> _eventTypes = new();

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public void AddSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (_handlers.ContainsKey(eventName))
            throw new InvalidOperationException($"Handler for {eventName} already registered.");
        

        _handlers[eventName] = handlerType;
        _eventTypes[eventName] = typeof(T);
    }

    public Type GetEventTypeForEvent(string eventName)
    {
        if (_eventTypes.TryGetValue(eventName, out var eventType))
            return eventType;

        throw new InvalidOperationException($"Event type for {eventName} not found.");
    }

    public Type GetHandlerTypeForEvent(string eventName)
    {
        if (_handlers.TryGetValue(eventName, out var handlerType)) 
            return handlerType;
        
        throw new InvalidOperationException($"Handler type for {eventName} not found.");
    }

    public string GetEventKey<T>() where T : IIntegrationEvent
    {
        return typeof(T).Name;
    }

    public void RemoveSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;

        if (!_handlers.ContainsKey(eventName))
            throw new InvalidOperationException($"No handler registered for {eventName}.");

        _handlers.Remove(eventName);
        _eventTypes.Remove(eventName);
    }
}
    
