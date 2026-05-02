using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Errors
{
    public static class CourseErrors
    {
        public static readonly Error TitleRequired =
            new("Course.TitleRequired", "Course title is required.");

        public static readonly Error DescriptionRequired =
            new("Course.DescriptionRequired", "Course description is required.");

        public static readonly Error InvalidPrice =
            new("Course.InvalidPrice", "Course price cannot be negative.");

        public static readonly Error InvalidInstructor =
            new("Course.InvalidInstructor", "A valid instructor ID is required.");

        public static readonly Error SectionTitleRequired =
            new("Course.SectionTitleRequired", "Section title is required.");

        public static readonly Error SectionNotFound =
            new("Course.SectionNotFound", "The section was not found.");

        public static readonly Error DuplicateSectionOrder =
            new("Course.DuplicateSectionOrder", "A section with this order already exists.");

        public static readonly Error NotFound =
            new("Course.NotFound", "The course was not found.");

        public static readonly Error AlreadyPublished =
            new("Course.AlreadyPublished", "The course is already published.");

        public static readonly Error AlreadyArchived =
            new("Course.AlreadyArchived", "The course is already archived.");

        public static readonly Error CannotPublishArchivedCourse =
            new("Course.CannotPublishArchived", "An archived course cannot be published.");

        public static readonly Error CannotModifyArchivedCourse =
            new("Course.CannotModifyArchived", "An archived course cannot be modified.");

        public static readonly Error NoSectionsToPublish =
            new("Course.NoSections", "A course must have at least one section before publishing.");

        public static readonly Error UnauthorizedInstructor =
            new("Course.Unauthorized", "You are not the instructor of this course.");
    }
}
