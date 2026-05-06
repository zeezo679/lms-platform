using Application.Interfaces;
using LMS.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUserRepository repository)
        : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var profile = await repository.GetByAuthUserIdAsync(request.AuthUserId, ct);

            if (profile == null)
                throw new DomainNotFoundException("UserProfile", request.AuthUserId);

            repository.Delete(profile);
            await repository.SaveChangesAsync(ct);
        }
    }
}
