namespace SoundCloudCloneApi.Domain.Entities;

public class Like
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid TrackId { get; set; }
    public Track Track { get; set; } = null!;
}