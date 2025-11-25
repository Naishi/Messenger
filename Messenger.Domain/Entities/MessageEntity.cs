using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class MessageEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int ChatId { get; set; }
    public ChatEntity Chat { get; set; } = null!;
    [MaxLength(16)] public required string AuthorName { get; set; }
    [MaxLength(256)] public required string Text { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    //public DateTime? ModifiedDate { get; set; }
    //public DateTime? TimeOfView { get; set; }
    //public int NumberOfViews { get; set; }
    public bool IsPinned { get; set; }
    //public ICollection<AttachmentEntity>? Attachments {  get; set; }
}