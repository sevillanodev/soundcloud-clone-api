using SoundCloudCloneApi.Application.DTOs.Auth;
using SoundCloudCloneApi.Application.Interfaces;
using SoundCloudCloneApi.Domain.Entities;
using SoundCloudCloneApi.Domain.Enums;
using SoundCloudCloneApi.Domain.Exceptions;
using SoundCloudCloneApi.Domain.Interfaces;

namespace SoundCloudCloneApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new ConflictException("Ya existe una cuenta registrada con ese correo.");

        if (await _userRepository.ExistsByUsernameAsync(request.Username))
            throw new ConflictException("Ese nombre de usuario ya está en uso.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            DisplayName = request.DisplayName,
            Role = request.IsArtist ? UserRole.Artist : UserRole.Listener,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Role = user.Role.ToString(),
            Token = token,
            ExpiresAt = expiresAt
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Role = user.Role.ToString(),
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}