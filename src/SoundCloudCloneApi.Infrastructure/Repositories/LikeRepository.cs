using Microsoft.EntityFrameworkCore;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;
using SoundCloudCloneApi.Infrastructure.Persistence;

namespace SoundCloudCloneApi.Infrastructure.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly ApplicationDbContext _context;

    public LikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Like?> GetAsync(Guid userId, Guid trackId) =>
        await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.TrackId == trackId);

    public async Task AddAsync(Like like) =>
        await _context.Likes.AddAsync(like);

    public void Remove(Like like) =>
        _context.Likes.Remove(like);

    public async Task<int> CountByTrackAsync(Guid trackId) =>
        await _context.Likes.CountAsync(l => l.TrackId == trackId);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}