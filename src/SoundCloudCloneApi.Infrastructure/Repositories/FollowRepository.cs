using Microsoft.EntityFrameworkCore;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;
using SoundCloudCloneApi.Infrastructure.Persistence;

namespace SoundCloudCloneApi.Infrastructure.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDbContext _context;

    public FollowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Follow?> GetAsync(Guid followerId, Guid followingId) =>
        await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

    public async Task AddAsync(Follow follow) =>
        await _context.Follows.AddAsync(follow);

    public void Remove(Follow follow) =>
        _context.Follows.Remove(follow);

    public async Task<List<User>> GetFollowersAsync(Guid userId) =>
        await _context.Follows
            .Where(f => f.FollowingId == userId)
            .Select(f => f.Follower)
            .ToListAsync();

    public async Task<List<User>> GetFollowingAsync(Guid userId) =>
        await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Following)
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}