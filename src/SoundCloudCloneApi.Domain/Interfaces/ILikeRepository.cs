using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Domain.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetAsync(Guid userId, Guid trackId);
    Task AddAsync(Like like);
    void Remove(Like like);
    Task<int> CountByTrackAsync(Guid trackId);
    Task SaveChangesAsync();
}