namespace Messenger.Service.Models;

public class RefreshTokenRequestModel
{
    public int Id { get; set; }

    public required string RefreshToken { get; set; }
}