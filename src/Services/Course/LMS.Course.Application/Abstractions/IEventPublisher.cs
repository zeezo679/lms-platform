using LMS.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Abstractions
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken ct = default)
            where T : IIntegrationEvent;
    }
}
