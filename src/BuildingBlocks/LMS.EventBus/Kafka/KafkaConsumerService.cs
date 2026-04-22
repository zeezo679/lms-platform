using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Confluent.Kafka;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LMS.EventBus.Kafka;

public class KafkaConsumerService : IHostedService, IDisposable
{

    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaConsumerService> _logger;
    private Task _executingTask = Task.CompletedTask;
    private CancellationTokenSource _cts = new();

    public KafkaConsumerService(KafkaSettings kafkaSettings, IServiceScopeFactory scopeFactory, ILogger<KafkaConsumerService> logger, IEventBusSubscriptionsManager subsManager)
    {
        _kafkaSettings = kafkaSettings;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _subsManager = subsManager;

        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            GroupId = kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false // we will commit manually after processing
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        var topics = _kafkaSettings.Topics; //list of topics the consumer subscribes to
        _consumer.Subscribe(topics);

        // start a background task to consume messages
        _executingTask = Task.Run(
             () => ExecuteAsync(_cts.Token), 
             cancellationToken
        );

        return Task.CompletedTask;
    }

    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(TimeSpan.FromSeconds(1));

                if (result?.Message?.Value is null) continue;

                await ProcessMessageAsync(result, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message");
            }
        }
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
    {
        var messageValue = consumeResult.Message.Value;

        var envelop = JsonSerializer.Deserialize<EventEnvelope>(messageValue);
        if (envelop is null) return;

        var eventName = envelop.EventType;

        // Step 2 — check if anyone handles this event
        if (!_subsManager.HasSubscriptionsForEvent(eventName))
        {
            _logger.LogWarning("No handler for event {EventName}", eventName);
            _consumer.Commit(consumeResult); // commit anyway — no point redelivering
            return;
        }

        //create a fresh DI scope for the message
        using var scope = _scopeFactory.CreateScope();

        var handlerType = _subsManager.GetHandlerTypeForEvent(eventName);
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        //deserialize the full event to its concrete type
        var eventType = _subsManager.GetEventTypeForEvent(eventName);
        var @event = JsonSerializer.Deserialize(messageValue, eventType);

        //invoke the Handle() method via reflection
        var handleMethod = handlerType.GetMethod("Handle");
        await (Task)handleMethod!.Invoke(handler, [@event, cancellationToken])!;

        _consumer.Commit(consumeResult);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        await _executingTask;
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}

internal record EventEnvelope(string EventType);
