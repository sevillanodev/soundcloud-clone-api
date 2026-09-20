using SoundCloudCloneApi.Application.DTOs.Common;
using SoundCloudCloneApi.Application.DTOs.Interactions;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Exceptions;
using SoundCloudCloneApi.Domain.Interfaces;

namespace SoundCloudCloneApi.Application.Services;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _followRepository;

    public FollowService(IFollowRepository followRepository)
    {
        _followRepository = followRepository;
    }

    public async Task<FollowResponseDto> ToggleFollowAsync(Guid followerId, Guid followingId)
    {
        if (followerId == followingId)
            throw new DomainException("No puedes seguirte a ti mismo.");

        var existing = await _followRepository.GetAsync(followerId, followingId);

        bool following;
        if (existing is not null)
        {
            _followRepository.Remove(existing);
            following = false;
        }
        else
        {
            await _followRepository.AddAsync(new Follow
            {
                Id = Guid.NewGuid(),
                FollowerId = followerId,
                FollowingId = followingId,
                CreatedAt = DateTime.UtcNow
            });
            following = true;
        }

        await _followRepository.SaveChangesAsync();
        return new FollowResponseDto { Following = following };
    }

    public async Task<List<UserSummaryDto>> GetFollowersAsync(Guid userId)
    {
        var users = await _followRepository.GetFollowersAsync(userId);
        return users.Select(MapToDto).ToList();
    }

    public async Task<List<UserSummaryDto>> GetFollowingAsync(Guid userId)
    {
        var users = await _followRepository.GetFollowingAsync(userId);
        return users.Select(MapToDto).ToList();
    }

    private static UserSummaryDto MapToDto(User u) => new()
    {
        Id = u.Id,
        Username = u.Username,
        DisplayName = u.DisplayName,
        ProfileImageUrl = u.ProfileImageUrl
    };
}