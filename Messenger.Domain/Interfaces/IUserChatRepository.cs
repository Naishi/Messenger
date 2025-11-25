using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;

public interface IUserChatRepository
{
    Task<UserChatEntity?> GetUserChatAsync(int userId, int chatId);

    Task AddUserChatAsync(UserChatEntity userChatEntity);

    Task DeleteUserChatAsync(UserChatEntity userChatEntity);

    Task AddUserChatEntityListAsync(List<UserChatEntity> userChatEntities);

    Task<List<UserChatEntity>> GetListUserChatsAsync(int chatid);

    Task<bool> CheckExistingChatAsync(int userId1, int userId2);

    Task<List<ChatEntity>> GetChatsByUserIdAsync(int userId);

    Task<List<UserEntity>?> GetUsersByChatIdAsync(int chatId);
}