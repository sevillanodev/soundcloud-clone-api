namespace SoundCloudCloneApi.Application.DTOs.Tracks;

public class TrackResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public int DurationSeconds { get; set; }
    public int PlaysCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public string ArtistUsername { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public Guid ArtistId { get; set; }
}