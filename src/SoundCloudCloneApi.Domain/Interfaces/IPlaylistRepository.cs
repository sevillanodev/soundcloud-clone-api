using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Domain.Interfaces;

public interface IPlaylistRepository
{
    Task<Playlist?> GetByIdAsync(Guid id);
    Task<List<Playlist>> GetByUserAsync(Guid userId);
    Task AddAsync(Playlist playlist);
    Task<PlaylistTrack?> GetPlaylistTrackAsync(Guid playlistId, Guid trackId);
    Task AddTrackAsync(PlaylistTrack playlistTrack);
    void RemoveTrack(PlaylistTrack playlistTrack);
    void Remove(Playlist playlist);
    Task SaveChangesAsync();
}