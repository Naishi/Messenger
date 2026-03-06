namespace Messenger.Domain.Filters;

public class MessageFilter
{
    public int? UserId { get; set; }
    
    public int? MessageId { get; set; }
    
    public int? ChatId { get; set; }
    
    public string? MessageText { get; set; }
}