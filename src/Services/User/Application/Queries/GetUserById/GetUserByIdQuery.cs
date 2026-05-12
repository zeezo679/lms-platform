using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserById
{
    public record GetUserByIdQuery(
        Guid Id) : IRequest<UserProfileDto>;
}
