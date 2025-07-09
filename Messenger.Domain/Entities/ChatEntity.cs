namespace Messenger.Domain.Entities;

public class ChatEntity
{
    public int Id {  get; set; }
    public string Name { get; set; }
    
    public DateOnly CreatedDate { get; set; }
    /*public bool IsPublic { get; set; }
    public string Description { get; set; } = String.Empty;
    public EChatType Type { get; set; }*/
    
    public ICollection<UserChatEntity> UserChats { get; set; }
}