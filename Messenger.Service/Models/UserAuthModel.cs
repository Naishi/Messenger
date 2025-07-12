using Messenger.Service.Models.Enums;

namespace Messenger.Service.Models;

public class UserAuthModel
{
    public int Id { get; set; }
    public string Email {  get; set; } = string.Empty;
    public string Password  {  get; set; } = string.Empty; 
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}