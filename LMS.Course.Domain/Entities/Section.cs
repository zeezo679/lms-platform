using LMS.Common.Entities;
using LMS.Course.Domain.Enums;
using LMS.Course.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Entities
{
    public class Section: BaseEntity
    {
        private Section() { }

        private readonly List<Lesson> _lessons = [];
        public Guid CourseId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public int Order {  get; private set; }
        public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        internal static Section Create(Guid courseId, string title, int order) =>
            new()
            {
                CourseId = courseId,
                Title = title,
                Order = order
            };

        internal Result AddLesson(
            string title,
            string? description,
            LessonType type,
            string? contentUrl,
            int durationInSeconds,
            int order,
            bool isFreePreview = false)
        {
            if (_lessons.Any(l => l.Order == order))
                return Result.Failure(LessonErrors.DuplicateOrder);

            var result = Lesson.Create(
                Id,
                title,
                description,
                type,
                contentUrl,
                durationInSeconds,
                order,
                isFreePreview);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            _lessons.Add(result.value);
            return Result.Success();
        }

        internal Result UpdateLesson(
            Guid lessonId,
            string title,
            string? description,
            LessonType type,
            string? contentUrl,
            int durationInSeconds,
            bool isFreePreview)
        {
            var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);

            if (lesson == null)
                return Result.Failure(LessonErrors.NotFound);

            return lesson.Update(title, description, type, contentUrl, durationInSeconds, isFreePreview);
        }

        internal Result RemoveLesson(Guid lessonId)
        {
            var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);

            if (lesson == null)
                return Result.Failure(LessonErrors.NotFound);

            _lessons.Remove(lesson);

            return Result.Success();
        }
    }
}
