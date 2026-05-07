using Application.Dtos;
using LMS.Common.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery(int PageNumber, int PageSize)
        : IRequest<PagedResponse<UserSummaryDto>>
    {
        //public GetAllUsersQuery(int pageNumber, int pageSize)
        //    : this(pageNumber < 1 ? 1 : pageNumber, pageSize < 1 ? 10 : pageSize) { }

        public int PageNumber { get; init; } = PageNumber < 1 ? 1 : PageNumber;
        public int PageSize { get; init; } = PageSize < 1 ? 10 : PageSize;
    }
}
