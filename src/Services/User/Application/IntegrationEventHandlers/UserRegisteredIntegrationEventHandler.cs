using Application.Commands.GetOrCreateUser;
using LMS.Contracts.Abstractions;
using LMS.Contracts.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationEventHandlers
{
    public class UserRegisteredIntegrationEventHandler(IMediator mediator,
        ILogger<UserRegisteredIntegrationEventHandler> logger) : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public async Task Handle(UserRegisteredIntegrationEvent @event, CancellationToken ct = default)
        {
            logger.LogInformation(
                "Handling UserRegisteredIntegrationEvent for UserId {UserId}, Email {Email}.",
                @event.UserId, @event.Email);

            var command = new GetOrCreateUserCommand(
                @event.UserId,
                @event.Email,
                @event.Role);

            await mediator.Send(command, ct);
        }
    }
}
