using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}