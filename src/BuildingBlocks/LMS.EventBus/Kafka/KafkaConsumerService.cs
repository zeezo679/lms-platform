using System;
using Confluent.Kafka;
using LMS.EventBus.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LMS.EventBus.Kafka;

public class KafkaConsumerService : IHostedService, IDisposable
{

    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaConsumerService> _logger;
    private Task _executingTask = Task.CompletedTask;
    private CancellationTokenSource _cts = new();

    public KafkaConsumerService(KafkaSettings kafkaSettings, IServiceScopeFactory scopeFactory, ILogger<KafkaConsumerService> logger)
    {
        _kafkaSettings = kafkaSettings;
        _scopeFactory = scopeFactory;
        _logger = logger;

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
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return _executingTask;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
