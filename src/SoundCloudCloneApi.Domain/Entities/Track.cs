using System.Xml.Linq;

namespace SoundCloudCloneApi.Domain.Entities;

public class Track
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string AudioFileUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public int DurationSeconds { get; set; }
    public int PlaysCount { get; set; } = 0;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Guid ArtistId { get; set; }
    public User Artist { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
}