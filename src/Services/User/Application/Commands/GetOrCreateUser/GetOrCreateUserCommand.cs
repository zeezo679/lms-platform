using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.GetOrCreateUser
{
    // Fired internally when a UserRegisteredIntegrationEvent arrives from the Auth service.
    public record GetOrCreateUserCommand(
        Guid AuthUserId,
        string Email,
        string Role) : IRequest<UserProfileDto>;
}
