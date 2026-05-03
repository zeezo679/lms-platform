using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Dtos.RequestDtos
{
    public class AddSectionDto
    {
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
