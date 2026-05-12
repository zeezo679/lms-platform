using LMS.Upload.Application.Interfaces;
using LMS.Upload.Application.Options;
using LMS.Upload.Domain.Entities;
using LMS.Upload.Domain.Enums;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LMS.Common.Exceptions;


namespace LMS.Upload.Application.Commands.UploadFile;

public sealed class UploadFileCommandHandler(
    IUploadDbContext context,
    IStorageProvider storageProvider,
    IEventBus eventBus,
    IOptions<StorageOptions> options,
    ILogger<UploadFileCommandHandler> logger) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var storageOptions = options.Value;

        // 1. Validate file size and MIME type per context
        ValidateFile(request.File, request.Context);

        // 2. Build storage path and upload
        var extension = Path.GetExtension(request.File.FileName);
        var uniqueFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{Guid.NewGuid()}{extension}";
        var relativePath = Path.Combine(request.Context.ToString(), request.UploadedBy.ToString(), uniqueFileName);

        await using var stream = request.File.OpenReadStream();
        var storedPath = await storageProvider.UploadFileAsync(stream, relativePath);

        // 3. Build public URL
        var fileUrl = $"{storageOptions.LocalStorageRootPath}/{relativePath}";

        // 4. Create and persist FileMetadata
        var metadata = FileMetadata.Create(
            id: Guid.NewGuid(),
            fileName: request.File.FileName,
            contentType: request.File.ContentType,
            uploadedAt: DateTime.UtcNow,
            uploadedBy: request.UploadedBy,
            storagePath: storedPath,
            fileUrl: fileUrl,
            size: request.File.Length,
            context: request.Context);

        await context.Files.AddAsync(metadata, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // 5. Publish event
        try
        {
            await eventBus.PublishAsync(
                new FileUploadedIntegrationEvent(
                    metadata.Id,
                    metadata.UploadedBy,
                    metadata.FileUrl,
                    metadata.Context.ToString(),
                    metadata.UploadedAt),
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "File {FileId} was saved but FileUploadedIntegrationEvent failed to publish. Manual intervention may be required.",
                metadata.Id);
        }

        // 6. Return URL to caller
        return fileUrl;
    }

    private static void ValidateFile(IFormFile file, UploadContext context)
    {
        var (maxBytes, allowedTypes) = context switch
        {
            UploadContext.UserProfilePicture => (5 * 1024 * 1024L, new[] { "image/jpeg", "image/png" }),
            UploadContext.AssignmentSubmission => (20 * 1024 * 1024L, new[] { "application/pdf", "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }),
            UploadContext.CourseMaterial => (500 * 1024 * 1024L, new[] { "application/pdf", "video/mp4" }),
            _ => throw new ArgumentOutOfRangeException(nameof(context))
        };

        if (file.Length > maxBytes)
            throw new DomainValidationException($"File exceeds the maximum allowed size for context '{context}'.");

        if (!allowedTypes.Contains(file.ContentType.ToLowerInvariant()))
            throw new DomainValidationException($"File type '{file.ContentType}' is not allowed for context '{context}'.");
    }
}