namespace Messenger.Dtos.UserDtos;

public class RegisterRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? NickName { get; set; }
}