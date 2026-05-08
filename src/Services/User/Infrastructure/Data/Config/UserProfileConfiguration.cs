using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
            .ValueGeneratedNever();

            // Each AuthUserId maps to exactly one profile
            builder.HasIndex(x => x.AuthUserId)
                .IsUnique();

            builder.HasIndex(x => x.Email)
            .IsUnique();

            builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

            builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

            builder.Property(u => u.LastModifiedAt);

            builder.Property(u => u.LastModifiedBy)
                .HasMaxLength(256);


            builder.Ignore(u => u.FullName);
        }
    }
}
