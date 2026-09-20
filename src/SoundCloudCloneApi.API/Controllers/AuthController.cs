using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundCloudCloneApi.Application.Common.Models;
using SoundCloudCloneApi.Application.DTOs.Auth;
using SoundCloudCloneApi.Application.Interfaces;

namespace SoundCloudCloneApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequestDto> _registerValidator;
    private readonly IValidator<LoginRequestDto> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequestDto> registerValidator,
        IValidator<LoginRequestDto> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.FailureResponse("Datos de registro inválidos.", errors));
        }

        var result = await _authService.RegisterAsync(request);
        return StatusCode(201, ApiResponse<AuthResponseDto>.SuccessResponse(result, "Usuario registrado correctamente."));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.FailureResponse("Datos de inicio de sesión inválidos.", errors));
        }

        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Inicio de sesión exitoso."));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(ApiResponse<object>.SuccessResponse(new { userId, username, role }, "Token válido."));
    }

    [Authorize(Roles = "Artist")]
    [HttpGet("artist-only")]
    public IActionResult ArtistOnly()
    {
        return Ok(ApiResponse<object>.SuccessResponse(new { message = "Acceso permitido solo para artistas." }));
    }
}