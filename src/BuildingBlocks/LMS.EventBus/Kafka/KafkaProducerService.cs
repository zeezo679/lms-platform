using System;
using Confluent.Kafka;
using LMS.EventBus.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LMS.EventBus.Kafka;

public class KafkaProducerService : IDisposable
{
    // thread safe, created once and reused throughout the application.
    // creating a producder is expensive (it opens a TCP connection to the Kafka broker)
    private readonly IProducer<string, string> _producer; 
    private readonly IOptions<KafkaSettings> _kafkaSettings;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
    { 
        _kafkaSettings = kafkaSettings;
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.Value.BootstrapServers, // "localhost:9092"
            Acks = Acks.All, // ensure message durability
            EnableIdempotence = true, // prevent duplicates
            MessageTimeoutMs = 5000
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string key, string value, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new Message<string, string> 
            { 
                Key = key, 
                Value = value 
            };
            
            var deliveryResult = await _producer.ProduceAsync(topic, message, cancellationToken);
            _logger.LogInformation("Message delivered to {TopicPartitionOffset}", deliveryResult.TopicPartitionOffset);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Failed to deliver message to topic {Topic}", topic);
            throw; // rethrow or handle as needed
        }
    }

    private bool _disposed = false;

    public void Dispose()
    {
        if (_disposed) return;
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
        _disposed = true;
    }
}
