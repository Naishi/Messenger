using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;

public interface IMessageRepository
{
    Task CreateMessageAsync(MessageEntity message, int userId);

    Task RemoveMessageAsync(int messageId, int userId);
    Task ModifyMessageAsync(int  messageId, string text, int userId);
    Task<List<MessageEntity>> GetAllMessageAsync(int chatId, int userId);

    Task<MessageEntity?> GetMessageAsync(int messageId);
}