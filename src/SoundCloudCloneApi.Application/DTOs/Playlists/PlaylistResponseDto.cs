using SoundCloudCloneApi.Application.DTOs.Tracks;

namespace SoundCloudCloneApi.Application.DTOs.Playlists;

public class PlaylistResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public string OwnerUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TrackResponseDto> Tracks { get; set; } = new();
}