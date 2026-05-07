using Application.Interfaces;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler(IUserRepository repository, IEventBus eventBus)
        : IRequestHandler<DeactivateUserCommand>
    {
        public async Task Handle(DeactivateUserCommand request, CancellationToken ct)
        {
            var profile = await repository.GetByAuthUserIdAsync(request.AuthUserId, ct);

            if (profile == null)
                throw new DomainNotFoundException("UserProfile", request.AuthUserId);

            profile.Deactivate();

            repository.Update(profile);
            await repository.SaveChangesAsync();

            try
            {
                await eventBus.PublishAsync(
                    new UserDeactivatedIntegrationEvent(
                        profile.Id,
                        profile.AuthUserId,
                        profile.Email), ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _ = ex;
            }
        }
    }
}
