using Messenger.Domain.Dto;
using Messenger.Service.Models;

namespace Messenger.Service.Services;

public interface IChatService
{
    Task AddChatAsync(ChatModel newChat, List<int> userIds);

    Task DeleteChatAsync(int chatId, int ownerId);

    Task<List<SearchChatEntity>> SearchChatsByCriteriaAsync(string search, int currentUserId);

    Task<List<ChatBasicModel>> GetChatsAsync(int userId);
}