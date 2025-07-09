namespace Messenger.Domain.Entities;

public class UserChatEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public int CompanionId { get; set; }
    public UserEntity Companion { get; set; }
    public int ChatId { get; set; }
    public ChatEntity Chat { get; set; }
    public bool IsPinned { get; set; }
    /*public EUserChatRole UserchatRole { get; set; }*/
}