using System;

namespace LMS.Upload.Application.Interfaces;

public interface IStorageProvider
{
    Task<string> UploadFileAsync(Stream fileStream, string filePath);
    Task DeleteFileAsync(string storagePath);
}
