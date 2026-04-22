using LMS.Common.Entities;
using LMS.Course.Domain.Enums;
using LMS.Course.Domain.Errors;
using LMS.Course.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Enums
{
    public sealed class Course: AuditableEntity
    {
        private readonly List<Section> _sections = new List<Section>();

        // Private Contructor use factory method to create course
        private Course() { }

        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string? ThumbnailUrl { get; private set; }
        public Guid InstructorId { get; private set; }
        public decimal Price { get; private set; }
        public CourseLevel Level { get; private set; }
        public CourseStatus Status { get; private set; } = CourseStatus.Draft;
        public string? Category { get; private set; }
        public IReadOnlyCollection<Section> Sections => _sections.AsReadOnly();


        private readonly List<object> _domainEvents = [];
        public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();


        // Factor Method to create course
        public static Result<Course> Create(
            string title,
            string description,
            Guid instructorId,
            decimal price,
            CourseLevel level,
            string? thumbnailUrl = null,
            string? category = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result<Course>.Failure(CourseErrors.TitleRequired);

            if (string.IsNullOrWhiteSpace(description))
                return Result<Course>.Failure(CourseErrors.DescriptionRequired);

            if (price < 0)
                return Result<Course>.Failure(CourseErrors.InvalidPrice);

            if (instructorId == Guid.Empty)
                return Result<Course>.Failure(CourseErrors.InvalidInstructor);

            var course = new Course
            {
                Title = title,
                Description = description.Trim(),
                InstructorId = instructorId,
                Price = price,
                Level = level,
                Category = category?.Trim(),
                ThumbnailUrl = thumbnailUrl,
                Status = CourseStatus.Draft
            };

            course._domainEvents.Add(new CourseCreatedEvent(course.Id, course.InstructorId, course.Title));

            return Result<Course>.Success(course);
        }

        public Result Update(
            string title,
            string description,
            decimal price,
            CourseLevel level,
            string? category,
            string? thumbnailUrl)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(CourseErrors.TitleRequired);

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure(CourseErrors.DescriptionRequired);

            if (price < 0)
                return Result.Failure(CourseErrors.InvalidPrice);

            Title = title.Trim();
            Description = description.Trim();
            Price = price;
            Level = level;
            Category = category?.Trim();
            ThumbnailUrl = thumbnailUrl;

            _domainEvents.Add(new CourseUpdatedEvent(Id, InstructorId, Title, Price));

            return Result.Success();
        }

        public Result Publish()
        {
            if (Status == CourseStatus.Published)
                return Result.Failure(CourseErrors.AlreadyPublished);

            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotPublishArchivedCourse);

            if (!_sections.Any())
                return Result.Failure(CourseErrors.NoSectionsToPublish);

            Status = CourseStatus.Published;

            _domainEvents.Add(new CoursePublishedEvent(Id, InstructorId, Title));

            return Result.Success();
        }

        public Result Archive()
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.AlreadyArchived);

            Status = CourseStatus.Archived;

            return Result.Success();
        }

        public Result AddSection(string title, int order)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(CourseErrors.SectionTitleRequired);

            _sections.Add(Section.Create(Id, title, order));

            return Result.Success();
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
