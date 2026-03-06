namespace Messenger.Domain.Filters;

public class UserChatFilter
{
    public ICollection<int> UserChatIds { get; set; } = [];
    
    public ICollection<int> ChatIds { get; set; } = [];
    
    public ICollection<int> UserIds { get; set; } = [];
}