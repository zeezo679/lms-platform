using System;

namespace LMS.Course.Domain.Errors;

public static class SubmissionErrors
{
    public static readonly Error InvalidLesson = new("Submission.InvalidLesson", "Lesson ID is invalid.");
    public static readonly Error InvalidStudent = new("Submission.InvalidStudent", "Student ID is invalid.");
    public static readonly Error FileUrlRequired = new("Submission.FileUrlRequired", "File URL is required.");
}