using AutoMapper;
using LMS.Contracts.Abstractions;
using LMS.Course.Application.Abstractions;
using LMS.Course.Application.Contracts;
using LMS.Course.Application.Dtos.RequestDtos;
using LMS.Course.Application.Dtos.Response_Dtos;
using LMS.Course.Domain;
using LMS.Course.Domain.Entities;
using LMS.Course.Domain.Enums;
using LMS.Course.Domain.Errors;
using LMS.Course.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Services
{
    public class CourseService: ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository repository,
            IUnitOfWork unitOfWork,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        //  Private helpers 

        private async Task PublishDomainEventsAsync(CourseEntity course, CancellationToken ct)
        {
            foreach (var domainEvent in course.DomainEvents.OfType<IIntegrationEvent>())
                await _eventPublisher.PublishAsync(domainEvent, ct);

            course.ClearDomainEvents();
        }

        // Queries
        public async Task<Result<PagedResult<CourseSummaryDto>>> GetCoursesAsync(
            int page, 
            int pageSize, 
            string? category, 
            CourseLevel? level, 
            string? search, 
            CancellationToken ct = default)
        {
            var safePage = Math.Max(1, page);
            var safePageSize = Math.Clamp(pageSize, 1, 100);

            var (items, totalCount) = await _repository.GetPagedAsync(safePage, safePageSize, category, level, search, ct);

            var dtos = _mapper.Map<List<CourseSummaryDto>>(items);

            return Result<PagedResult<CourseSummaryDto>>.Success(
                new PagedResult<CourseSummaryDto>(dtos, safePage, safePageSize, totalCount));
        }
        public async Task<Result<CourseDto>> GetCourseByIdAsync(Guid courseId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result<CourseDto>.Failure(CourseErrors.NotFound);

            return Result<CourseDto>.Success(_mapper.Map<CourseDto>(course));
        }

        // Course commands
        public async Task<Result<Guid>> CreateCourseAsync(CreateCourseDto dto, Guid instructorId, CancellationToken ct = default)
        {
            var result = CourseEntity.Create(
                dto.Title,
                dto.Description,
                instructorId,
                dto.Price,
                dto.Level,
                dto.Category,
                dto.ThumbnailUrl);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var course = result.value;

            await _repository.AddAsync(course, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await PublishDomainEventsAsync(course, ct);

            return Result<Guid>.Success(course.Id);
        }
        public async Task<Result> UpdateCourseAsync(Guid courseId, UpdateCourseDto dto, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.Update(
                dto.Title,
                dto.Description,
                dto.Price,
                dto.Level,
                dto.Category,
                dto.ThumbnailUrl);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);
            await PublishDomainEventsAsync(course, ct);

            return Result.Success();
        }
        public async Task<Result> DeleteCourseAsync(Guid courseId, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            _repository.Delete(course);
            await _unitOfWork.SaveChangesAsync(ct);

            await _eventPublisher.PublishAsync(
                new CourseDeletedEvent(course.Id, course.InstructorId), ct);

            return Result.Success();
        }
        public async Task<Result> PublishCourseAsync(Guid courseId, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.Publish();

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);
            await PublishDomainEventsAsync(course, ct);

            return Result.Success();
        }

        // Section commands
        public async Task<Result> AddSectionAsync(Guid courseId, AddSectionDto dto, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.AddSection(dto.Title, dto.Order);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
        public async Task<Result> RemoveSectionAsync(Guid courseId, Guid sectionId, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.RemoveSection(sectionId);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        // Lesson commands
        public async Task<Result> AddLessonAsync(Guid courseId, AddLessonDto dto, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.AddLesson(
                dto.SectionId,
                dto.Title,
                dto.Description,
                dto.Type,
                dto.ContentUrl,
                dto.DurationInSeconds,
                dto.Order,
                dto.IsFreePreview);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
        public async Task<Result> UpdateLessonAsync(Guid courseId, Guid sectionId, Guid lessonId, UpdateLessonDto dto, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.UpdateLesson(
                sectionId,
                lessonId,
                dto.Title,
                dto.Description,
                dto.Type,
                dto.ContentUrl,
                dto.DurationInSeconds,
                dto.IsFreePreview);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> RemoveLessonAsync(Guid courseId, Guid sectionId, Guid lessonId, Guid requestingInstructorId, CancellationToken ct = default)
        {
            var course = await _repository.GetByIdAsync(courseId, ct);

            if (course is null)
                return Result.Failure(CourseErrors.NotFound);

            if (course.InstructorId != requestingInstructorId)
                return Result.Failure(CourseErrors.UnauthorizedInstructor);

            var result = course.RemoveLesson(sectionId, lessonId);

            if (result.IsFailure)
                return result;

            _repository.Update(course);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
