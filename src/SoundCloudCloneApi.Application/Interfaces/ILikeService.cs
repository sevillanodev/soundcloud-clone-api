using SoundCloudCloneApi.Application.DTOs.Interactions;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface ILikeService
{
    Task<LikeResponseDto> ToggleLikeAsync(Guid userId, Guid trackId);
}