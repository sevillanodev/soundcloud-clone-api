using SoundCloudCloneApi.Application.DTOs.Tracks;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Exceptions;
using SoundCloudCloneApi.Domain.Interfaces;

namespace SoundCloudCloneApi.Application.Services;

public class TrackService : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IFileStorageService _fileStorageService;

    public TrackService(ITrackRepository trackRepository, IFileStorageService fileStorageService)
    {
        _trackRepository = trackRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<TrackResponseDto> UploadTrackAsync(UploadTrackDto dto, Guid artistId)
    {
        var audioUrl = await _fileStorageService.SaveAudioFileAsync(dto.AudioFile);
        string? coverUrl = dto.CoverImage is not null
            ? await _fileStorageService.SaveCoverImageAsync(dto.CoverImage)
            : null;

        var track = new Track
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Genre = dto.Genre,
            DurationSeconds = dto.DurationSeconds,
            AudioFileUrl = audioUrl,
            CoverImageUrl = coverUrl,
            ArtistId = artistId,
            UploadedAt = DateTime.UtcNow
        };

        await _trackRepository.AddAsync(track);
        await _trackRepository.SaveChangesAsync();

        var savedTrack = await _trackRepository.GetByIdAsync(track.Id);

        return MapToDto(savedTrack!);
    }

    public async Task<List<TrackResponseDto>> GetAllTracksAsync()
    {
        var tracks = await _trackRepository.GetAllAsync();
        return tracks.Select(MapToDto).ToList();
    }

    public async Task<List<TrackResponseDto>> SearchTracksAsync(string? query, string? genre, string? artist)
    {
        var tracks = await _trackRepository.SearchAsync(query, genre, artist);
        return tracks.Select(MapToDto).ToList();
    }

    public async Task<(Track Track, string RelativePath)> GetTrackForStreamingAsync(Guid trackId)
    {
        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track is null)
            throw new EntityNotFoundException("Track", trackId);

        return (track, track.AudioFileUrl);
    }

    private static TrackResponseDto MapToDto(Track t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Genre = t.Genre,
        DurationSeconds = t.DurationSeconds,
        PlaysCount = t.PlaysCount,
        CoverImageUrl = t.CoverImageUrl,
        ArtistUsername = t.Artist.Username,
        ArtistId = t.ArtistId,
        UploadedAt = t.UploadedAt
    };
}