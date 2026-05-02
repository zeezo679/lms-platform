using AutoMapper;
using LMS.Course.Application.Dtos.Response_Dtos;
using LMS.Course.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Application.Mapping
{
    public class CourseProfile: Profile
    {
        public CourseProfile()
        {
            // Lesson → LessonDto
            CreateMap<Lesson, LessonDto>();

            // Section → SectionDto 
            // Lessons are ordered by Order before mapping
            CreateMap<Section, SectionDto>()
                .ForMember(
                    dest => dest.Lessons,
                    opt => opt.MapFrom(src => src.Lessons.OrderBy(l => l.Order)));

            //  Course → CourseDto 
            // Sections are ordered by Order before mapping
            CreateMap<CourseEntity, CourseDto>()
                .ForMember(
                    dest => dest.Sections,
                    opt => opt.MapFrom(src => src.Sections.OrderBy(s => s.Order)));

            //  Course → CourseSummaryDto
            // Computed fields are derived from the nested collections
            CreateMap<CourseEntity, CourseSummaryDto>()
                .ForMember(
                    dest => dest.SectionCount,
                    opt => opt.MapFrom(src => src.Sections.Count))
                .ForMember(
                    dest => dest.TotalLessonCount,
                    opt => opt.MapFrom(src => src.Sections.Sum(s => s.Lessons.Count)))
                .ForMember(
                    dest => dest.TotalDurationInSeconds,
                    opt => opt.MapFrom(src =>
                        src.Sections.Sum(s => s.Lessons.Sum(l => l.DurationInSeconds))));
        }
    }
}
