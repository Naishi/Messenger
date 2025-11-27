using Messenger.Service.Models;

namespace Messenger.Service.Services;

public interface IMessageService
{
    Task CreateMessageAsync(MessageModel message, int  userId);

    Task RemoveMessageAsync(int messageId, int userId);

    Task ModifyMessageAsync(int messageId, string text, int userId);

    
    Task<List<MessageModel>> GetAllMessagesAsync(int userId, int chatId);

    Task<MessageModel> GetMessageAsync(int messageId, int userId);
}