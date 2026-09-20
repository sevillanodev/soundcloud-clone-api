using SoundCloudCloneApi.Domain.Enums;
using System.Diagnostics;
using System.Xml.Linq;

namespace SoundCloudCloneApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public UserRole Role { get; set; } = UserRole.Listener;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();

    // Seguidores: usuarios que me siguen a mí
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    // Seguidos: usuarios a los que yo sigo
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
}