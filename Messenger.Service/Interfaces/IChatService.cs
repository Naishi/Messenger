using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IChatService
{
    Task AddChatAsync(ChatModel newChat, List<int> userIds);

    Task DeleteChatAsync(int chatId, int ownerId);

    Task<List<ChatModel>> SearchChatsByCriteriaAsync(string search, int currentUserId);

    Task<List<ChatModel>> GetChatsAsync(int userId);
}