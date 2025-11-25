namespace Messenger.Service.Models;

public class MessageModel
{

    public int UserId { get; set; }

    public int ChatId { get; set; }
    public string AuthorName { get; set; }
    public required string Text { get; set; }

    public DateTime CreatedDate { get; set; }

    //public int NumberOfViews { get; set; }
    public bool IsPinned { get; set; }
}