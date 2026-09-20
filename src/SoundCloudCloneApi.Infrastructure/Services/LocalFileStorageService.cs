using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SoundCloudCloneApi.Application.Common.Models;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Exceptions;

namespace SoundCloudCloneApi.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _settings;
    private static readonly string[] AllowedAudioExtensions = { ".mp3", ".wav" };
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxAudioSizeBytes = 50 * 1024 * 1024; // 50 MB
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;  // 5 MB

    public LocalFileStorageService(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> SaveAudioFileAsync(IFormFile file)
    {
        ValidateFile(file, AllowedAudioExtensions, MaxAudioSizeBytes, "audio");
        return await SaveFileAsync(file, _settings.AudioBasePath);
    }

    public async Task<string> SaveCoverImageAsync(IFormFile file)
    {
        ValidateFile(file, AllowedImageExtensions, MaxImageSizeBytes, "imagen");
        return await SaveFileAsync(file, _settings.CoverBasePath);
    }

    public void DeleteFile(string relativePath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public (Stream Stream, string ContentType, long Length) GetFileStream(string relativePath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        if (!File.Exists(fullPath))
            throw new EntityNotFoundException("Archivo", relativePath);

        var contentType = GetContentType(Path.GetExtension(fullPath));
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return (stream, contentType, stream.Length);
    }

    private static void ValidateFile(IFormFile file, string[] allowedExtensions, long maxSizeBytes, string tipoArchivo)
    {
        if (file is null || file.Length == 0)
            throw new DomainException($"El archivo de {tipoArchivo} está vacío o no fue enviado.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            throw new DomainException($"Formato de {tipoArchivo} no permitido. Formatos válidos: {string.Join(", ", allowedExtensions)}");

        if (file.Length > maxSizeBytes)
            throw new DomainException($"El archivo de {tipoArchivo} excede el tamaño máximo permitido de {maxSizeBytes / 1024 / 1024} MB.");
    }

    private static async Task<string> SaveFileAsync(IFormFile file, string basePath)
    {
        var fullDirectory = Path.Combine(Directory.GetCurrentDirectory(), basePath);
        Directory.CreateDirectory(fullDirectory);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(fullDirectory, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(basePath, fileName).Replace("\\", "/");
    }

    private static string GetContentType(string extension) => extension.ToLowerInvariant() switch
    {
        ".mp3" => "audio/mpeg",
        ".wav" => "audio/wav",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        _ => "application/octet-stream"
    };
}