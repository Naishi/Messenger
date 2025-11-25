namespace Messenger.Dtos.MessageDtos;

public class CreateMessageDto
{
    public int ChatId { get; set; }
    public required string Text { get; set; }
    //public List<IFormFile> Attachments { get; set; } = [];
}