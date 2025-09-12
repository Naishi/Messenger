namespace Messenger.Service.Models;

public class MessageModel
{
    public int Id {  get; set; }
    public UserModel User {  get; set; } = null!;
    public ChatModel Chat {  get; set; } = null!;
    public required string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    //public int NumberOfViews { get; set; }
    public bool IsPinned { get; set; }
}