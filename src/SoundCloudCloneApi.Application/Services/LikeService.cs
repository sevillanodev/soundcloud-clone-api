using SoundCloudCloneApi.Application.DTOs.Interactions;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Interfaces;

namespace SoundCloudCloneApi.Application.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;

    public LikeService(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task<LikeResponseDto> ToggleLikeAsync(Guid userId, Guid trackId)
    {
        var existingLike = await _likeRepository.GetAsync(userId, trackId);

        bool liked;
        if (existingLike is not null)
        {
            _likeRepository.Remove(existingLike);
            liked = false;
        }
        else
        {
            await _likeRepository.AddAsync(new Like
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TrackId = trackId,
                CreatedAt = DateTime.UtcNow
            });
            liked = true;
        }

        await _likeRepository.SaveChangesAsync();
        var count = await _likeRepository.CountByTrackAsync(trackId);

        return new LikeResponseDto { Liked = liked, LikesCount = count };
    }
}