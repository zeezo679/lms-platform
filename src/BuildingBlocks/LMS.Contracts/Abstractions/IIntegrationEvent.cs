using System;

namespace LMS.Contracts.Abstractions;

/*
An integration event is a message used in event-driven architecture 
to communicate state changes or significant 
occurrences across different microservices or bounded contexts
*/
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}


