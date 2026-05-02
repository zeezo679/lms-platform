using LMS.Common.Entities;
using LMS.Enrollment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Infrastructure.Data
{
    public class EnrollmentDbContext : DbContext
    {
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options)
            : base(options)
        {
        }
        // DbSet for StudentEnrollments
        public DbSet<StudentEnrollment> Enrollments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the StudentEnrollment entity
            modelBuilder.Entity<StudentEnrollment>(entity =>
            {
                entity.ToTable("StudentEnrollments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentId).IsRequired();
                entity.Property(e => e.CourseId).IsRequired();
                entity.Property(e => e.EnrollmentDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
