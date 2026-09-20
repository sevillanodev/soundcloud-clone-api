using Microsoft.EntityFrameworkCore;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;
using SoundCloudCloneApi.Infrastructure.Persistence;

namespace SoundCloudCloneApi.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly ApplicationDbContext _context;

    public PlaylistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Playlist?> GetByIdAsync(Guid id) =>
        await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                    .ThenInclude(t => t.Artist)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Playlist>> GetByUserAsync(Guid userId) =>
        await _context.Playlists
            .Include(p => p.PlaylistTracks)
            .Where(p => p.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(Playlist playlist) =>
        await _context.Playlists.AddAsync(playlist);

    public async Task<PlaylistTrack?> GetPlaylistTrackAsync(Guid playlistId, Guid trackId) =>
        await _context.PlaylistTracks.FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

    public async Task AddTrackAsync(PlaylistTrack playlistTrack) =>
        await _context.PlaylistTracks.AddAsync(playlistTrack);

    public void RemoveTrack(PlaylistTrack playlistTrack) =>
        _context.PlaylistTracks.Remove(playlistTrack);

    public void Remove(Playlist playlist) =>
        _context.Playlists.Remove(playlist);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}