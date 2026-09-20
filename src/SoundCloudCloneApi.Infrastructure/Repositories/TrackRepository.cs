using Microsoft.EntityFrameworkCore;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;
using SoundCloudCloneApi.Infrastructure.Persistence;

namespace SoundCloudCloneApi.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly ApplicationDbContext _context;

    public TrackRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Track?> GetByIdAsync(Guid id) =>
        await _context.Tracks.Include(t => t.Artist).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<Track>> GetAllAsync() =>
        await _context.Tracks.Include(t => t.Artist).OrderByDescending(t => t.UploadedAt).ToListAsync();

    public async Task<List<Track>> SearchAsync(string? query, string? genre, string? artistUsername)
    {
        var tracks = _context.Tracks.Include(t => t.Artist).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            tracks = tracks.Where(t => t.Title.Contains(query));

        if (!string.IsNullOrWhiteSpace(genre))
            tracks = tracks.Where(t => t.Genre != null && t.Genre.Contains(genre));

        if (!string.IsNullOrWhiteSpace(artistUsername))
            tracks = tracks.Where(t => t.Artist.Username.Contains(artistUsername));

        return await tracks.OrderByDescending(t => t.UploadedAt).ToListAsync();
    }

    public async Task AddAsync(Track track) =>
        await _context.Tracks.AddAsync(track);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}