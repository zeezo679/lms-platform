using LMS.Course.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace LMS.Course.Infrastructure.Data
{
    public class CourseAppDbContext: DbContext
    {
        public CourseAppDbContext(DbContextOptions<CourseAppDbContext> options)
            :base(options) { }

        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseAppDbContext).Assembly);
        }

    }
}
