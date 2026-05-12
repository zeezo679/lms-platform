using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Configuration.RefreshTokenConfig;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token).IsRequired();

        builder.Property(rt => rt.UserId).IsRequired();

        builder.Property(rt => rt.ExpiresAt).IsRequired();

        builder.Property(rt => rt.IsRevoked).IsRequired();

        builder.HasOne(rt => rt.user)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
