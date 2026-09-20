
using Microsoft.AspNetCore.Http;

namespace SoundCloudCloneApi.Application.DTOs.Tracks;

public class UploadTrackDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public int DurationSeconds { get; set; }
    public IFormFile AudioFile { get; set; } = null!;
    public IFormFile? CoverImage { get; set; }
}