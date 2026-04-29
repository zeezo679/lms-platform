using LMS.Common.Entities;
using LMS.Course.Domain.Enums;
using LMS.Course.Domain.Errors;
using LMS.Course.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Entities
{
    public sealed class CourseEntity: AuditableEntity
    {

        // Private Contructor use factory method to create course
        private CourseEntity() { }

        private readonly List<Section> _sections = [];
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
        public static Result<CourseEntity> Create(
            string title,
            string description,
            Guid instructorId,
            decimal price,
            CourseLevel level,
            string? thumbnailUrl = null,
            string? category = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result<CourseEntity>.Failure(CourseErrors.TitleRequired);

            if (string.IsNullOrWhiteSpace(description))
                return Result<CourseEntity>.Failure(CourseErrors.DescriptionRequired);

            if (price < 0)
                return Result<CourseEntity>.Failure(CourseErrors.InvalidPrice);

            if (instructorId == Guid.Empty)
                return Result<CourseEntity>.Failure(CourseErrors.InvalidInstructor);

            var course = new CourseEntity
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

            return Result<CourseEntity>.Success(course);
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

            if (_sections.Any(s => s.Order == order))
                return Result.Failure(CourseErrors.DuplicateSectionOrder);

            var section = Section.Create(Id, title, order);

            _sections.Add(section);

            _domainEvents.Add(new SectionAddedEvent(Id, section.Id, section.Title));

            return Result.Success();
        }

        public Result RemoveSection(Guid sectionId)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            var section = _sections.FirstOrDefault(s => s.Id == sectionId);

            if (section is null)
                return Result.Failure(CourseErrors.SectionNotFound);

            _sections.Remove(section);

            _domainEvents.Add(new SectionRemovedEvent(Id, section.Id));

            return Result.Success();
        }

        public Result AddLesson(
           Guid sectionId,
           string title,
           string? description,
           LessonType type,
           string? contentUrl,
           int durationInSeconds,
           int order,
           bool isFreePreview = false)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            var section = _sections.FirstOrDefault(s => s.Id == sectionId);

            if (section is null)
                return Result.Failure(CourseErrors.SectionNotFound);

            //_domainEvents.Add(new LessonAddedEvent(Id, section.Id, lesson.Id))

            return section.AddLesson(
                title, description, type, contentUrl, durationInSeconds, order, isFreePreview);
        }

        public Result UpdateLesson(
            Guid sectionId,
            Guid lessonId,
            string title,
            string? description,
            LessonType type,
            string? contentUrl,
            int durationInSeconds,
            bool isFreePreview)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            var section = _sections.FirstOrDefault(s => s.Id == sectionId);

            if (section is null)
                return Result.Failure(CourseErrors.SectionNotFound);

            return section.UpdateLesson(
                lessonId, title, description, type, contentUrl, durationInSeconds, isFreePreview);
        }

        public Result RemoveLesson(Guid sectionId, Guid lessonId)
        {
            if (Status == CourseStatus.Archived)
                return Result.Failure(CourseErrors.CannotModifyArchivedCourse);

            var section = _sections.FirstOrDefault(s => s.Id == sectionId);

            if (section is null)
                return Result.Failure(CourseErrors.SectionNotFound);

            return section.RemoveLesson(lessonId);
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
