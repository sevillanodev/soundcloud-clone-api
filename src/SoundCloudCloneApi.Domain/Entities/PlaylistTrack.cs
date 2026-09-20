namespace SoundCloudCloneApi.Domain.Entities;

public class PlaylistTrack
{
    public Guid PlaylistId { get; set; }
    public Playlist Playlist { get; set; } = null!;

    public Guid TrackId { get; set; }
    public Track Track { get; set; } = null!;

    public int Order { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}