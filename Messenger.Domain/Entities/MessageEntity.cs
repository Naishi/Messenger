namespace Messenger.Domain.Entities;

public class MessageEntity
{
    public int Id {  get; set; }
    public int UserId {  get; set; }
    public int ChatId {  get; set; }
    public required string AuthorName {  get; set; }
    public required string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int NumberOfViews { get; set; }
    public bool IsPinned { get; set; }
}