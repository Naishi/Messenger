using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class ChatEntity
{
    public int Id {  get; set; }
    public string Name { get; set; }
    /*public EChatType Type { get; set; }*/
    public DateOnly CreatedDate { get; set; }
    /*public bool IsPublic { get; set; }
    public string Description { get; set; } = String.Empty;*/
}