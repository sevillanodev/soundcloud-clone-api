using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}