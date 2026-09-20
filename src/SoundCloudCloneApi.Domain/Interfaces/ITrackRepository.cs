using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Domain.Interfaces;

public interface ITrackRepository
{
    Task<Track?> GetByIdAsync(Guid id);
    Task<List<Track>> GetAllAsync();
    Task<List<Track>> SearchAsync(string? query, string? genre, string? artistUsername);
    Task AddAsync(Track track);
    Task SaveChangesAsync();
}