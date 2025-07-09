namespace Messenger.Domain.Entities;

public class UserAuthEntity
{
    public int Id { get; set; }
    public string Email {  get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}