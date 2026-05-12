using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using LMS.Common.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(IUserRepository repository)
        : IRequestHandler<GetAllUsersQuery, PagedResponse<UserSummaryDto>>
    {
        public async Task<PagedResponse<UserSummaryDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            var (items, totalCount) = await repository.GetPagedAsync(request.PageNumber,
                request.PageSize, ct);

            var dtos = items.Select(MapToSummary).ToList();

            return new PagedResponse<UserSummaryDto>(dtos, request.PageNumber, request.PageSize, totalCount);
        }
        private static UserSummaryDto MapToSummary(UserProfile profile) =>
            new(
                profile.Id,
                profile.AuthUserId,
                profile.Email,
                profile.FullName,
                profile.Role,
                profile.Status);
    }
}
