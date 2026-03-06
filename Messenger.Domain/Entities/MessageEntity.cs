using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class MessageEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public required UserEntity User { get; set; }

    public int ChatId { get; set; }

    public ChatEntity Chat { get; set; } = null!;

    [MaxLength(16)]
    public required string AuthorName { get; set; }
    
    [MaxLength(256)]
    public required string Text { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsPinned { get; set; }
}