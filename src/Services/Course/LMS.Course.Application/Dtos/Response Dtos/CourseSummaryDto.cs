using LMS.Course.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Dtos.Response_Dtos
{
    public class CourseSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl {  get; set; }
        public Guid InstructorId { get; set; }
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; }
        public CourseStatus Status { get; set; }
        public string? Category { get; set; }
        public int SectionCount { get; set; }
        public int TotalLessonCount { get; set; }
        public int TotalDurationInSeconds { get; set; }
    }
}
