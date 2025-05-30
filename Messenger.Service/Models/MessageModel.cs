namespace Messenger.Service.Models;

public class MessageModel
{
    public int Id {  get; set; }
    public int UserChatId { get; set; }
    public required string AuthorName {  get; set; }
    public required string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int NumberOfViews { get; set; }
    public bool IsPinned { get; set; }
}