namespace Messenger.Domain.Filters;

public class ContactFilter
{
    public int? OwnerUserId { get; set; }
    
    public int? ContactUserId { get; set; }
    
    public string? DisplayName { get; set; }
}