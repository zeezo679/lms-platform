using System;
using System.Text.Json;
using LMS.Contracts.Abstractions;
using LMS.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace LMS.EventBus.Kafka;

public class KafkaEventBus : IEventBus
{

    private readonly KafkaProducerService _producerService;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly ILogger<KafkaEventBus> _logger;

    public KafkaEventBus(KafkaProducerService producerService, IEventBusSubscriptionsManager subsManager, ILogger<KafkaEventBus> logger)
    {
        _producerService = producerService;
        _subsManager = subsManager;
        _logger = logger;
    }

    //produce the event to Kafka topic. the topic name is determined by the event type name
    public Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : IIntegrationEvent
    {
        _logger.LogInformation("Publishing event {EventName} with id {EventId}", typeof(T).Name, @event.EventId);

        var topic = _subsManager.GetEventKey<T>();
        var key = @event.EventId.ToString();
        var value = JsonSerializer.Serialize(@event);

        return _producerService.ProduceAsync(topic, key, value, ct);
    }

    public void Subscribe<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _subsManager.AddSubscription<T, TH>();
        _logger.LogInformation("Subscribed to event {EventName} with handler {HandlerName}", typeof(T).Name, typeof(TH).Name);
    }

    public void UnsubscribeAsync<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _subsManager.RemoveSubscription<T, TH>();
        _logger.LogInformation("Unsubscribed from event {EventName} with handler {HandlerName}", typeof(T).Name, typeof(TH).Name);
    }
}
