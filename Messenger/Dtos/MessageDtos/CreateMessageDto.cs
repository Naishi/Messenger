namespace Messenger.Dtos.MessageDtos;

public class CreateMessageDto
{
    public int ChatId { get; set; }

    public required string Text { get; set; }
}