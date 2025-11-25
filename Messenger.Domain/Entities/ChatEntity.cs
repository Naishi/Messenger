using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class ChatEntity
{
    public int Id { get; set; }
    [MaxLength(64)]
    public required string Name { get; set; }

    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    /*public bool IsPublic { get; set; }
    public string Description { get; set; } = String.Empty;
    public EChatType Type { get; set; }*/

    public required ICollection<UserChatEntity> UserChats { get; set; } = new List<UserChatEntity>();
    public ICollection<MessageEntity>? Messages { get; set; }
    public DateTime LastMessageDate { get; set; }
}