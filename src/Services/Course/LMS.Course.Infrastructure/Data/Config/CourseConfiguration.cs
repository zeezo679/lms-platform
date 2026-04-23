using LMS.Course.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Course.Infrastructure.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
    {
        public void Configure(EntityTypeBuilder<CourseEntity> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever(); // Guid is set in domain

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(x => x.Level)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasMany(c => c.Sections)
                .WithOne()
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Sections)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_sections");

            builder.Ignore(x => x.DomainEvents);

            builder.HasIndex(c => c.InstructorId);
            builder.HasIndex(c => c.Status);
            builder.HasIndex(c => c.Category);
            builder.HasIndex(c => c.CreatedAt);
        }
    }
}
