using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Dtos.Response_Dtos
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order {  get; set; }
        public IReadOnlyList<LessonDto> Lessons = new List<LessonDto>();
    }
}
