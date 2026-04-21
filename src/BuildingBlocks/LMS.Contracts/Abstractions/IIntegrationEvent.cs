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
    DateTime CreationDate { get; }
}

/*

Benefits of the IIntegrationEvent Interface
In a microservices project, using a standardized interface like IIntegrationEvent provides several architectural advantages:

Infrastructure Abstraction: It allows the application to remain technology-agnostic. 
By defining an interface, you can easily swap between different message brokers (e.g., from RabbitMQ to Kafka) without changing the core business logic.

Polymorphic Dispatching: A common interface enables a centralized event bus to handle various event types uniformly. 
This simplifies the implementation of generic Publish and Subscribe methods within the infrastructure layer.

Loose Coupling: It enforces a clear boundary between the application layer and the underlying messaging infrastructure. 
Services interact with the abstraction, which minimizes dependency chains and facilitates independent deployment.

Standardized Metadata: It provides a place to enforce mandatory metadata across all events, 
such as a unique EventId, CreationDate, or CorrelationId, which is vital for logging, auditing, and debugging in distributed systems.

Improved Testability: By using an interface, 
developers can easily mock events during unit tests, ensuring that the service's reaction to an event can be verified without requiring a live message broker

*/
