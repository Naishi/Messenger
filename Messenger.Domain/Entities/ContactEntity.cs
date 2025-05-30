namespace Messenger.Domain.Entities;

public class ContactEntity
{
    public int Id { get; set; }
    public int OwnerUserId { get; set; }
    public int ContactUserId { get; set; }
    public string DisplayName { get; set; }
}