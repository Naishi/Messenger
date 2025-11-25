using Messenger.Domain.Dto;
using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;


public interface IChatRepository
{
    Task CreateChatAsync(ChatEntity chat);
    Task UpdateChatAsync(ChatEntity chat);
    Task DeleteChatAsync(int id);
    Task<ChatEntity?> GetChatAsync(int id);
    Task<List<UserChatEntity>> GetAllChatsAsync(int userId);

    Task<List<SearchChatEntity>> SearchChatByCriteriaAsync(int currentUserId, string searchTerm);

    Task UpdateLastMessageTime(int chatId);
}