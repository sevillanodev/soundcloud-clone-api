using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundCloudCloneApi.Application.Common.Models;
using SoundCloudCloneApi.Application.DTOs.Playlists;
using SoundCloudCloneApi.Application.Interfaces;

namespace SoundCloudCloneApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    private Guid CurrentUserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlaylistDto dto)
    {
        var result = await _playlistService.CreateAsync(dto, CurrentUserId);
        return StatusCode(201, ApiResponse<PlaylistResponseDto>.SuccessResponse(result, "Playlist creada correctamente."));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _playlistService.GetByIdAsync(id);
        return Ok(ApiResponse<PlaylistResponseDto>.SuccessResponse(result));
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var result = await _playlistService.GetMyPlaylistsAsync(CurrentUserId);
        return Ok(ApiResponse<List<PlaylistResponseDto>>.SuccessResponse(result));
    }

    [HttpPost("{id}/tracks")]
    public async Task<IActionResult> AddTrack(Guid id, [FromBody] AddTrackToPlaylistDto dto)
    {
        await _playlistService.AddTrackAsync(id, dto.TrackId, CurrentUserId);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Track agregado a la playlist."));
    }

    [HttpDelete("{id}/tracks/{trackId}")]
    public async Task<IActionResult> RemoveTrack(Guid id, Guid trackId)
    {
        await _playlistService.RemoveTrackAsync(id, trackId, CurrentUserId);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Track eliminado de la playlist."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _playlistService.DeleteAsync(id, CurrentUserId);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Playlist eliminada."));
    }
}