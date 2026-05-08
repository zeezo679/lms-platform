using LMS.Contracts.Abstractions;

namespace LMS.Contracts.Events;

public sealed record FileUploadedIntegrationEvent(
    Guid FileId,
    Guid UploadedBy,
    string FileUrl,
    string Context,
    DateTime UploadedAt,
    Guid EventId,
    DateTime OccurredOn) : IIntegrationEvent
{
    public FileUploadedIntegrationEvent(
        Guid fileId,
        Guid uploadedBy,
        string fileUrl,
        string context,
        DateTimeOffset uploadedAt)
        : this(fileId, uploadedBy, fileUrl, context, uploadedAt.UtcDateTime, Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}
