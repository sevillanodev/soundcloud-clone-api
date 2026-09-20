namespace SoundCloudCloneApi.Application.DTOs.Common;

public class UserSummaryDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}