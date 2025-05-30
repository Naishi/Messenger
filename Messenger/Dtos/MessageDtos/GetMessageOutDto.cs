namespace Messenger.Dtos.MessageDtos;

public class GetMessageOutDto
{
    public required string AutorName { get; set; }

    public required string Text { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int NumberOfViews { get; set; }

    public bool IsPinned { get; set; }
}