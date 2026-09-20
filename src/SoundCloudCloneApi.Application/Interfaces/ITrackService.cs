using SoundCloudCloneApi.Application.DTOs.Tracks;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface ITrackService
{
    Task<TrackResponseDto> UploadTrackAsync(UploadTrackDto dto, Guid artistId);
    Task<List<TrackResponseDto>> GetAllTracksAsync();
    Task<List<TrackResponseDto>> SearchTracksAsync(string? query, string? genre, string? artist);
    Task<(Domain.Entities.Track Track, string RelativePath)> GetTrackForStreamingAsync(Guid trackId);
}