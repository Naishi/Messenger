using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class UserAuthEntity
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Email { get; set; }

    [MaxLength(512)]
    public required string PasswordHash { get; set; }

    [MaxLength(32)]
    public string Role { get; set; } = string.Empty;
    
    [MaxLength(64)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}