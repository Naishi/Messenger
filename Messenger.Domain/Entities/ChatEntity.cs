using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class ChatEntity
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Name { get; set; }

    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public required ICollection<UserChatEntity?> UserChats { get; set; } = [];

    public ICollection<MessageEntity>? Messages { get; set; }

    public DateTime LastMessageDate { get; set; }
}