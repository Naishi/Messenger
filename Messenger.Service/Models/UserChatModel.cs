namespace Messenger.Service.Models;

public class UserChatModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanionId { get; set; }

    public int ChatId { get; set; }

    public bool IsPinned { get; set; }
}