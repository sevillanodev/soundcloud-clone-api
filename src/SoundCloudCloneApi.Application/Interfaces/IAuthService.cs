using SoundCloudCloneApi.Application.DTOs.Auth;

namespace SoundCloudCloneApi.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}