using LMS.Course.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Dtos.RequestDtos
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; }
        public string? Category { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
