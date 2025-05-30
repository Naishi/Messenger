namespace Messenger.Dtos.MessageDtos;

public class CreateMessageDto
{
    public int Id { get; set; }

    public int UserChatId { get; set; }

    public required string AuthorName { get; set; }

    public required string Text { get; set; }

    public DateTime CreatedDate { get; set; }
}