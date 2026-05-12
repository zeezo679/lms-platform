using LMS.Common.Responses;
using LMS.Enrollment.Application.DTOs;
using LMS.Enrollment.Application.Interfaces.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Queries.GetAllEnrollments
{
    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, PagedResponse<StudentEnrollmentsGroupDto>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public GetAllEnrollmentsQueryHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<PagedResponse<StudentEnrollmentsGroupDto>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var (data, totalRecords) = await _enrollmentRepository.GetPagedGroupedEnrollmentsAsync(request.PageNumber, request.PageSize);

            return new PagedResponse<StudentEnrollmentsGroupDto>(data, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}