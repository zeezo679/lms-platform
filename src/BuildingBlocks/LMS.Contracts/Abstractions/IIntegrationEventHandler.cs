using System;

namespace LMS.Contracts.Abstractions;


// this method contains the logic to handle the event. it will be called by the event bus when the event is published.
public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task Handle(TEvent @event, CancellationToken ct = default); //default value is CacellationToken.None
}
