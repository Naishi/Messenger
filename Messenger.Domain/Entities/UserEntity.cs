using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Email { get; set; }
    [MaxLength(16)]
    public required string Name { get; set; }
    [MaxLength(256)]
    public string? Description { get; set; }

    public DateOnly? Birthday { get; set; }

    [MaxLength(64)]
    public string? NickName { get; set; }
    
    /// <summary>
    /// Связь многие ко многим между пользователями и чатами 
    /// </summary>
    public ICollection<UserChatEntity> UserChats { get; set; }
    
    /// <summary>
    /// Контакты добавленные пользователем
    /// </summary>
    public ICollection<ContactEntity> Contacts { get; set; }
    
    /// <summary>
    /// Контакты, которые добавили пользователя
    /// </summary>
    public ICollection<ContactEntity> AddedBy { get; set; }
}