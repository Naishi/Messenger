namespace Messenger.Domain.Filters;

public class UserFilter
{
    public ICollection<int?> UserIds { get; set; } = [];
    
    public string? Search { get; set; }
    
    public SearchType? SearchType { get; set; }
    
    public int? ChatId { get; set; }
}