using Microsoft.EntityFrameworkCore;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;
using SoundCloudCloneApi.Infrastructure.Persistence;

namespace SoundCloudCloneApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<bool> ExistsByEmailAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task<bool> ExistsByUsernameAsync(string username) =>
        await _context.Users.AnyAsync(u => u.Username == username);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}