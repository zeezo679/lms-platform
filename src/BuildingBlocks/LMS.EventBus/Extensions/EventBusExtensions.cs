using System;
using LMS.EventBus.Abstractions;
using LMS.EventBus.Configuration;
using LMS.EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.EventBus.Extensions;

public static class EventBusExtensions
{   
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new KafkaSettings();
        configuration.GetSection("Kafka").Bind(options);

        // register the event bus and its dependencies
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddSingleton<IEventBusSubscriptionsManager, InMemorySubscriptionsManager>();
        services.AddSingleton<KafkaProducerService>();
        services.AddSingleton<KafkaEventBus>();
        services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<KafkaEventBus>());
        services.AddHostedService<KafkaConsumerService>();

        return services;
    }
}
