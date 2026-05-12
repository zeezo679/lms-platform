using LMS.Common.Responses;
using LMS.Enrollment.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Queries.GetAllEnrollments
{
    public class GetAllEnrollmentsQuery : IRequest<PagedResponse<StudentEnrollmentsGroupDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllEnrollmentsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 10 : pageSize;
        }
    }
}
