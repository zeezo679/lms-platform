# LMS Microservices Architecture Rules & Guidelines

Welcome to the Learning Management System (LMS) development team. As we scale our microservices architecture using ASP.NET Core, Kafka, and Docker, maintaining a cohesive system requires strict alignment on certain architectural boundaries, while leaving room for teams to innovate within their own domains.

This document outlines our engineering standards, divided into two tiers: the Non-Negotiable "Constitution" and the Flexible "Team Autonomy Zone."

---

## Tier 1: Non-Negotiable Standards (Must Follow Rules)

These rules are strict and mandatory across all services. Violating these rules will either break the distributed system architecture, cause cascading failures, or introduce severe security vulnerabilities. There are no exceptions without a formal architectural review.

### 1. Services Communicate ONLY via Kafka Events or HTTP Contracts
* **The Rule:** You may never add a direct project reference from one microservice project to another.
* **Why It's Non-Negotiable:** Direct references create tight compile-time and runtime coupling, which collapses the microservices architecture back into a distributed monolith. 
* **Implementation:** Use Apache Kafka for asynchronous, event-driven communication (e.g., `CoursePublishedEvent`). For synchronous operations where immediate responses are mandatory, use well-defined HTTP/REST or gRPC contracts.

### 2. All Shared Event Schemas Live in `LMS.Contracts`
* **The Rule:** All Kafka event models, DTOs used across service boundaries, and integration events must be defined in a centralized repository/package (e.g., an `LMS.Contracts` NuGet package).
* **Why It's Non-Negotiable:** If each service defines its own `UserRegisteredEvent`, they will inevitably diverge. When a producer changes a property name or type, the consumer will fail silently during deserialization, causing untraceable runtime failures.
* **Implementation:** Keep this project dependency-free. It should only contain POCOs (Plain Old C# Objects) and interfaces.

### 3. Each Service Owns Its Own Database
* **The Rule:** No two microservices may connect to the same database schema or instance. 
* **Why It's Non-Negotiable:** Shared databases create invisible coupling at the data layer. If the Course team alters a table structure, it could instantly break the Enrollment service.
* **Implementation:** Apply the Database-per-Service pattern. If the Enrollment service needs course data, it must either query the Course service via HTTP or subscribe to Kafka events to keep a localized, read-optimized projection of the data.

### 4. APIs Must Be Versioned
* **The Rule:** Every public-facing and internal API route must explicitly state its version (e.g., `/api/v1/courses`).
* **Why It's Non-Negotiable:** Unversioned APIs mean that a simple change made by the Course team can silently break the API Gateway, frontend applications, or downstream services.
* **Implementation:** Use the `Microsoft.AspNetCore.Mvc.Versioning` package. Default to v1 if unspecified, but enforce explicit versioning for all new endpoints.

### 5. Authentication is Validated at the Gateway Only
* **The Rule:** Downstream microservices do not validate JWTs, verify signatures, or handle OAuth flows.
* **Why It's Non-Negotiable:** If every service re-implements JWT validation differently, we introduce a massive security risk and create redundant processing overhead.
* **Implementation:** The API Gateway (e.g., Ocelot, YARP, or an external gateway) authenticates the user. It then strips the token and passes the validated claims to the downstream services via secure HTTP headers (e.g., `X-User-Id`, `X-User-Roles`).

---

## Tier 2: Flexible Decisions (Team Autonomy Zone)

These are decisions where development teams have full autonomy. Because these choices do not leak across service boundaries, teams can optimize for their specific domain requirements.

### 1. Internal Architecture Style
* **The Flexibility:** You can choose Clean Architecture, Vertical Slice Architecture, Onion Architecture, or a simple 3-tier approach.
* **Why It's Flexible:** This is an internal concern. The Course service's controllers do not know or care how the Enrollment service organizes its folders or namespaces. Optimize for your team's velocity and the domain's complexity.

### 2. Database Engine Choice (Polyglot Persistence)
* **The Flexibility:** You can use SQL Server, PostgreSQL, MongoDB, Redis, etc.
* **Why It's Flexible:** The fundamental promise of microservices is polyglot persistence. The Course service might handle complex, unstructured JSON documents perfectly suited for MongoDB, while the Financial/Payment service demands the strict ACID compliance of a relational database like SQL Server.

### 3. ORM or Data Access Strategy
* **The Flexibility:** Choose between Entity Framework Core (EF Core), Dapper, or raw ADO.NET.
* **Why It's Flexible:** Data access is an infrastructure detail that should be hidden behind a repository or handler interface. If a service requires high-performance bulk inserts, Dapper is fine; if it relies heavily on complex relational tracking, EF Core is great.

### 4. Unit Testing Framework
* **The Flexibility:** xUnit, NUnit, or MSTest.
* **Why It's Flexible:** Testing frameworks do not impact the deployed artifact or the distributed system's behavior. (Though standardizing on xUnit is generally recommended for ASP.NET Core, it is not strictly enforced).

### 5. Internal CQRS Implementation
* **The Flexibility:** MediatR, Brighter, or manual interface dispatching.
* **Why It's Flexible:** How a service routes an HTTP request to its internal business logic handler is completely opaque to the rest of the system.

---
# Questions you might ask
## what is a kafka event model?
## what is an event schema?
formal, structured blueprint that defines the format, data types, and constraints of the messages (events) exchanged between decoupled services