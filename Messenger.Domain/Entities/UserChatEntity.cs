namespace Messenger.Domain.Entities;

public class UserChatEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CompanionId { get; set; }
    public int ChatId { get; set; }
    /*public EUserChatRole UserchatRole { get; set; }*/
    public bool IsPinned { get; set; }
}