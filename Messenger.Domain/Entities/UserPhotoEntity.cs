namespace Messenger.Domain.Entities;

public class UserPhotoEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}