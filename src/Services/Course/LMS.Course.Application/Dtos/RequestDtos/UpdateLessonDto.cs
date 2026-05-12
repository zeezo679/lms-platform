using LMS.Course.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Dtos.RequestDtos
{
    public class UpdateLessonDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LessonType Type { get; set; }
        public string? ContentUrl { get; set; }
        public int DurationInSeconds { get; set; }
        public bool IsFreePreview { get; set; } = false;
    }
}
