using System;
using System.ComponentModel.DataAnnotations;
using LMS.Upload.Domain.Enums;

namespace LMS.Upload.Domain.Entities;

public class FileMetadata
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public Guid UploadedBy { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }
    public string StoragePath { get; private set; } = null!;
    public long Size { get; private set; }
    public string FileUrl { get; private set; } = null!;
    public UploadContext Context { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    public DateTimeOffset? DeletedAt { get; private set; }

    private FileMetadata()
    {
    }

    public static FileMetadata Create(
        Guid id,
        string fileName,
        string contentType,
        Guid uploadedBy,
        DateTimeOffset uploadedAt,
        string storagePath,
        long size,
        string fileUrl,
        UploadContext context)
    {
        return new FileMetadata
        {
            Id = id,
            FileName = fileName,
            ContentType = contentType,
            UploadedBy = uploadedBy,
            UploadedAt = uploadedAt,
            StoragePath = storagePath,
            Size = size,
            FileUrl = fileUrl,
            Context = context,
            IsDeleted = false,
            DeletedAt = null
        };
    }
} 
