using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Course.Infrastructure.Data.Config;

public class SubmissionConfig : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.LessonId)
            .IsRequired();

        builder.Property(s => s.StudentId)
            .IsRequired();

        builder.Property(s => s.FileUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(s => s.SubmittedAt)
            .IsRequired();

        builder.Property(s => s.Grade)
            .IsRequired(false);
    }

}