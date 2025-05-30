namespace Messenger.Dtos.ChatDtos;

public class ChangeChatDto
{
    public required int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsPublic { get; set; }

    public string? Description { get; set; }
}