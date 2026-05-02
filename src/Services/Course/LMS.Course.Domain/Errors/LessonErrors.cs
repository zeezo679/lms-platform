using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Errors
{
    public static class LessonErrors
    {
        public static readonly Error TitleRequired =
            new("Lesson.TitleRequired", "Lesson title is required.");

        public static readonly Error NotFound =
            new("Lesson.NotFound", "The lesson was not found.");

        public static readonly Error InvalidDuration =
            new("Lesson.InvalidDuration", "Lesson duration cannot be negative.");

        public static readonly Error DuplicateOrder =
            new("Lesson.DuplicateOrder", "A lesson with this order already exists in the section.");
    }
}
