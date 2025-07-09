namespace Messenger.Domain.Entities;

public class UserEntity
{
    public int Id {  get; set; }
    public required string PhoneNumber { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public DateOnly Birthday { get; set; }
    public string NickName { get; set; }
    public ICollection<UserChatEntity> UserChats { get; set; }
}