using LMS.Common.Entities;
using LMS.Course.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Domain.Enums
{
    public class Lesson: BaseEntity
    {
        private Lesson() { }

        public Guid SectionId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public LessonType Type { get; private set; }
        public string? ContentUrl { get; private set; }  // video/pdf/external link
        public int DurationInSeconds { get; private set; } // 0 if not applicable
        public int Order {  get; private set; }
        public bool IsFreePreview { get; private set; }

        internal static Result<Lesson> Create(
            Guid sectionId,
            string title,
            string? description,
            LessonType type,
            string? contentUrl,
            int durationInSeconds,
            int order,
            bool isFreePreview = false)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result<Lesson>.Failure(LessonErrors.TitleRequired);

            if (durationInSeconds < 0)
                return Result<Lesson>.Failure(LessonErrors.InvalidDuration);

            return Result<Lesson>.Success(new Lesson
            {
                SectionId = sectionId,
                Title = title.Trim(),
                Description = description?.Trim(),
                Type = type,
                ContentUrl = contentUrl?.Trim(),
                DurationInSeconds = durationInSeconds,
                Order = order,
                IsFreePreview = isFreePreview
            });
        }

        internal Result Update(
           string title,
           string? description,
           LessonType type,
           string? contentUrl,
           int durationInSeconds,
           bool isFreePreview)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(LessonErrors.TitleRequired);

            if (durationInSeconds < 0)
                return Result.Failure(LessonErrors.InvalidDuration);

            Title = title.Trim();
            Description = description?.Trim();
            Type = type;
            ContentUrl = contentUrl?.Trim();
            DurationInSeconds = durationInSeconds;
            IsFreePreview = isFreePreview;

            return Result.Success();
        }
    }
}
