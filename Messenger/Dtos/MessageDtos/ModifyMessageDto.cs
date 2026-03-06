namespace Messenger.Dtos.MessageDtos;

public class ModifyMessageDto
{
    public int MessageId { get; set; }

    public required string Text { get; set; }
}