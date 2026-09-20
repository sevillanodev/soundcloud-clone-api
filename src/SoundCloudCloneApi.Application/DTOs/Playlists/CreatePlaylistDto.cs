namespace SoundCloudCloneApi.Application.DTOs.Playlists;

public class CreatePlaylistDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublic { get; set; } = true;
}