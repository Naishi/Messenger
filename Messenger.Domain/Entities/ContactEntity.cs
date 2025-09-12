using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class ContactEntity
{
    public int Id { get; set; }
    public int OwnerUserId { get; set; }
    public UserEntity OwnerUser { get; set; }
    public int ContactUserId { get; set; }
    public UserEntity ContactUser { get; set; }
    [MaxLength(32)]
    public string? DisplayName { get; set; }
}