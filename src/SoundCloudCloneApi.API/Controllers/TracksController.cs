using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundCloudCloneApi.Application.Common.Models;
using SoundCloudCloneApi.Application.DTOs.Tracks;
using SoundCloudCloneApi.Application.Interfaces;

namespace SoundCloudCloneApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TracksController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tracks = await _trackService.GetAllTracksAsync();
        return Ok(ApiResponse<List<TrackResponseDto>>.SuccessResponse(tracks));
    }

    [Authorize(Roles = "Artist")]
    [HttpPost("upload")]
    [RequestSizeLimit(60_000_000)] // ~60 MB límite de request completo
    public async Task<IActionResult> Upload([FromForm] UploadTrackDto dto)
    {
        var artistId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _trackService.UploadTrackAsync(dto, artistId);
        return StatusCode(201, ApiResponse<TrackResponseDto>.SuccessResponse(result, "Track subido correctamente."));
    }

    [HttpGet("{id}/stream")]
    public async Task<IActionResult> Stream(Guid id)
    {
        var (track, relativePath) = await _trackService.GetTrackForStreamingAsync(id);

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        if (!System.IO.File.Exists(fullPath))
            return NotFound(ApiResponse<object>.FailureResponse("El archivo de audio no existe en el servidor."));

        var contentType = Path.GetExtension(fullPath).ToLowerInvariant() switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            _ => "application/octet-stream"
        };

        // PhysicalFile con enableRangeProcessing:true maneja automáticamente
        // las cabeceras Range del navegador y responde con HTTP 206 Partial Content
        return PhysicalFile(fullPath, contentType, enableRangeProcessing: true);
    }
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> ToggleLike(Guid id, [FromServices] IServiceProvider serviceProvider)
    {
        var likeService = serviceProvider.GetRequiredService<SoundCloudCloneApi.Application.Interfaces.ILikeService>();
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await likeService.ToggleLikeAsync(userId, id);
        return Ok(ApiResponse<SoundCloudCloneApi.Application.DTOs.Interactions.LikeResponseDto>.SuccessResponse(result));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? query, [FromQuery] string? genre, [FromQuery] string? artist)
    {
        var tracks = await _trackService.SearchTracksAsync(query, genre, artist);
        return Ok(ApiResponse<List<TrackResponseDto>>.SuccessResponse(tracks));
    }
}