namespace Messenger.Domain.Dto;

public class SearchChatEntity
{
    public int Id { get; set; }

    public DateOnly CreatedDate { get; set; }

    public string DisplayName { get; set; }
}