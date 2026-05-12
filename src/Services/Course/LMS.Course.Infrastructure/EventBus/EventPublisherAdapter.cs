using LMS.Contracts.Abstractions;
using LMS.Course.Application.Abstractions;
using LMS.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Infrastructure.EventBus
{
    public class EventPublisherAdapter : IEventPublisher
    {
        private readonly IEventBus _eventBus;
        public EventPublisherAdapter(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : IIntegrationEvent
            =>  _eventBus.PublishAsync(@event);
    }
}
