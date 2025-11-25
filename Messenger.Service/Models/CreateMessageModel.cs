namespace Messenger.Service.Models;

public class CreateMessageModel
{
    public int ChatId { get; set; }
    public int Userid { get; set; }
    public required string Text { get; set; }
    //public List<AttachmentModel> Attachments { get; set; } = [];
}