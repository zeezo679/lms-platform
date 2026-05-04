using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.GetOrCreateUser
{
    public class GetOrCreateUserCommandHandler(
        IUserRepository repository,
        ILogger<GetOrCreateUserCommandHandler> logger) : IRequestHandler<GetOrCreateUserCommand, UserProfileDto>
    {
        public Task<UserProfileDto> Handle(GetOrCreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
