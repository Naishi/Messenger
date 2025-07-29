namespace Messenger.Service.Models;

public class UserModel
{
    public int Id {  get; set; }
    public required string  Email { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? NickName { get; set; }
}