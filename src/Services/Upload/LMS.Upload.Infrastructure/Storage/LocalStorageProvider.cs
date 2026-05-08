using System;
using LMS.Upload.Application.Interfaces;
using LMS.Upload.Application.Options;
using Microsoft.Extensions.Options;

namespace LMS.Upload.Infrastructure.Storage;

public class LocalStorageProvider : IStorageProvider
{
    private readonly StorageOptions _options;

    public LocalStorageProvider(IOptions<StorageOptions> options)
    {
        _options = options.Value;
    }
    public async Task<string> UploadFileAsync(Stream fileStream, string filePath)
    {
        var fullPath = Path.Combine(_options.LocalStorageRootPath, filePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var fileStreamOutput = File.Create(fullPath);
        await fileStream.CopyToAsync(fileStreamOutput);

        return fullPath;
    }
    public Task DeleteFileAsync(string storagePath)
    {
        if (File.Exists(storagePath))
        {
            File.Delete(storagePath);
        }
        return Task.CompletedTask;
    }

}
