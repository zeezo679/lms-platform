using Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler(IUserRepository repository)
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
        }
    }
}
