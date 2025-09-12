using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class UserPhotoEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}