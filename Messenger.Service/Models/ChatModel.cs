using Messenger.Domain.Entities;

namespace Messenger.Service.Models;

public class ChatModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public DateOnly CreatedDate { get; set; }

    public List<MessageEntity> Messages { get; set; } = null!;
}