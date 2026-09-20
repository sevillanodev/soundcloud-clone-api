using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundCloudCloneApi.Application.Common.Models;
using SoundCloudCloneApi.Application.DTOs.Common;
using SoundCloudCloneApi.Application.DTOs.Interactions;
using SoundCloudCloneApi.Application.Interfaces;

namespace SoundCloudCloneApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IFollowService _followService;

    public UsersController(IFollowService followService)
    {
        _followService = followService;
    }

    [Authorize]
    [HttpPost("{id}/follow")]
    public async Task<IActionResult> ToggleFollow(Guid id)
    {
        var followerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _followService.ToggleFollowAsync(followerId, id);
        return Ok(ApiResponse<FollowResponseDto>.SuccessResponse(result));
    }

    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetFollowers(Guid id)
    {
        var followers = await _followService.GetFollowersAsync(id);
        return Ok(ApiResponse<List<UserSummaryDto>>.SuccessResponse(followers));
    }

    [HttpGet("{id}/following")]
    public async Task<IActionResult> GetFollowing(Guid id)
    {
        var following = await _followService.GetFollowingAsync(id);
        return Ok(ApiResponse<List<UserSummaryDto>>.SuccessResponse(following));
    }
}