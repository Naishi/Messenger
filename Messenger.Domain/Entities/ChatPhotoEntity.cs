namespace Messenger.Domain.Entities;

public class ChatPhotoEntity
{
    public int Id {  get; set; }
    public int ChatId {  get; set; }
    public string Name {  get; set; }
    public bool IsActive {  get; set; }
}