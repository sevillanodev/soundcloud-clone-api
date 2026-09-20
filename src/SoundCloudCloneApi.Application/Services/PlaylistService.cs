using SoundCloudCloneApi.Application.DTOs.Playlists;
using SoundCloudCloneApi.Application.DTOs.Tracks;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Exceptions;
using SoundCloudCloneApi.Domain.Interfaces;

namespace SoundCloudCloneApi.Application.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ITrackRepository _trackRepository;

    public PlaylistService(IPlaylistRepository playlistRepository, ITrackRepository trackRepository)
    {
        _playlistRepository = playlistRepository;
        _trackRepository = trackRepository;
    }

    public async Task<PlaylistResponseDto> CreateAsync(CreatePlaylistDto dto, Guid userId)
    {
        var playlist = new Playlist
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsPublic = dto.IsPublic,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _playlistRepository.AddAsync(playlist);
        await _playlistRepository.SaveChangesAsync();

        var saved = await _playlistRepository.GetByIdAsync(playlist.Id);
        return MapToDto(saved!);
    }

    public async Task<PlaylistResponseDto> GetByIdAsync(Guid playlistId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        if (playlist is null)
            throw new EntityNotFoundException("Playlist", playlistId);

        return MapToDto(playlist);
    }

    public async Task<List<PlaylistResponseDto>> GetMyPlaylistsAsync(Guid userId)
    {
        var playlists = await _playlistRepository.GetByUserAsync(userId);
        var result = new List<PlaylistResponseDto>();

        foreach (var p in playlists)
        {
            var full = await _playlistRepository.GetByIdAsync(p.Id);
            result.Add(MapToDto(full!));
        }

        return result;
    }

    public async Task AddTrackAsync(Guid playlistId, Guid trackId, Guid userId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        if (playlist is null)
            throw new EntityNotFoundException("Playlist", playlistId);

        if (playlist.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta playlist.");

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track is null)
            throw new EntityNotFoundException("Track", trackId);

        var existing = await _playlistRepository.GetPlaylistTrackAsync(playlistId, trackId);
        if (existing is not null)
            throw new ConflictException("Este track ya está en la playlist.");

        await _playlistRepository.AddTrackAsync(new PlaylistTrack
        {
            PlaylistId = playlistId,
            TrackId = trackId,
            Order = playlist.PlaylistTracks.Count,
            AddedAt = DateTime.UtcNow
        });

        await _playlistRepository.SaveChangesAsync();
    }

    public async Task RemoveTrackAsync(Guid playlistId, Guid trackId, Guid userId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        if (playlist is null)
            throw new EntityNotFoundException("Playlist", playlistId);

        if (playlist.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta playlist.");

        var playlistTrack = await _playlistRepository.GetPlaylistTrackAsync(playlistId, trackId);
        if (playlistTrack is null)
            throw new EntityNotFoundException("PlaylistTrack", trackId);

        _playlistRepository.RemoveTrack(playlistTrack);
        await _playlistRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid playlistId, Guid userId)
    {
        var playlist = await _playlistRepository.GetByIdAsync(playlistId);
        if (playlist is null)
            throw new EntityNotFoundException("Playlist", playlistId);

        if (playlist.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para eliminar esta playlist.");

        _playlistRepository.Remove(playlist);
        await _playlistRepository.SaveChangesAsync();
    }

    private static PlaylistResponseDto MapToDto(Playlist p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        IsPublic = p.IsPublic,
        OwnerUsername = p.User.Username,
        CreatedAt = p.CreatedAt,
        Tracks = p.PlaylistTracks.Select(pt => new TrackResponseDto
        {
            Id = pt.Track.Id,
            Title = pt.Track.Title,
            Description = pt.Track.Description,
            Genre = pt.Track.Genre,
            DurationSeconds = pt.Track.DurationSeconds,
            PlaysCount = pt.Track.PlaysCount,
            CoverImageUrl = pt.Track.CoverImageUrl,
            ArtistUsername = pt.Track.Artist.Username,
            UploadedAt = pt.Track.UploadedAt
        }).ToList()
    };
}