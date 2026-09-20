using Microsoft.AspNetCore.Http;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAudioFileAsync(IFormFile file);
    Task<string> SaveCoverImageAsync(IFormFile file);
    void DeleteFile(string relativePath);
    (Stream Stream, string ContentType, long Length) GetFileStream(string relativePath);
}