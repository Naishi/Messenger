using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;

public interface IUserChatRepository
{
    Task<UserChatEntity?> GetUserChatEntity(int userId, int chatId);
}