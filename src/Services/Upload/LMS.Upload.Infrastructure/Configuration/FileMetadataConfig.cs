using LMS.Upload.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Upload.Infrastructure.Configuration;

public class FileMetadataConfig : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileName).IsRequired().HasMaxLength(255);
        builder.Property(f => f.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(f => f.StoragePath).IsRequired();
        builder.Property(f => f.FileUrl).IsRequired();
        builder.Property(f => f.Size).IsRequired();
        builder.Property(f => f.Context).HasConversion<string>().IsRequired();
        builder.Property(f => f.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(f => f.DeletedAt);
    }
}
