using SoundCloudCloneApi.Application.DTOs.Common;
using SoundCloudCloneApi.Application.DTOs.Interactions;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface IFollowService
{
    Task<FollowResponseDto> ToggleFollowAsync(Guid followerId, Guid followingId);
    Task<List<UserSummaryDto>> GetFollowersAsync(Guid userId);
    Task<List<UserSummaryDto>> GetFollowingAsync(Guid userId);
}