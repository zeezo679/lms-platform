using System;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Configuration;
using LMS.EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.EventBus.Extensions;

public static class EventBusExtensions
{   
    public static IServiceCollection AddEventBusProducer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddSingleton<IEventBusSubscriptionsManager, InMemorySubscriptionsManager>();
        services.AddSingleton<KafkaProducerService>();
        services.AddSingleton<KafkaEventBus>();
        services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<KafkaEventBus>());

        return services;
    }

    public static IServiceCollection AddEventBusProducerAndConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventBusProducer(configuration);
        services.AddHostedService<KafkaConsumerService>();

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddEventBusProducerAndConsumer(configuration);
    }
}
