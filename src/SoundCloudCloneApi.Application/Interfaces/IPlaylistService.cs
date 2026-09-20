using SoundCloudCloneApi.Application.DTOs.Playlists;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface IPlaylistService
{
    Task<PlaylistResponseDto> CreateAsync(CreatePlaylistDto dto, Guid userId);
    Task<PlaylistResponseDto> GetByIdAsync(Guid playlistId);
    Task<List<PlaylistResponseDto>> GetMyPlaylistsAsync(Guid userId);
    Task AddTrackAsync(Guid playlistId, Guid trackId, Guid userId);
    Task RemoveTrackAsync(Guid playlistId, Guid trackId, Guid userId);
    Task DeleteAsync(Guid playlistId, Guid userId);
}