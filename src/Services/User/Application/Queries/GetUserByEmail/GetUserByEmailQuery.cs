using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(
        string Email) : IRequest<UserProfileDto>;
}
