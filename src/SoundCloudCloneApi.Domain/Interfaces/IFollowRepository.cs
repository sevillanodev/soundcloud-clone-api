using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Domain.Interfaces;

public interface IFollowRepository
{
    Task<Follow?> GetAsync(Guid followerId, Guid followingId);
    Task AddAsync(Follow follow);
    void Remove(Follow follow);
    Task<List<User>> GetFollowersAsync(Guid userId);
    Task<List<User>> GetFollowingAsync(Guid userId);
    Task SaveChangesAsync();
}