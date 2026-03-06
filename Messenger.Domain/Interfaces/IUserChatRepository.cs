using Messenger.Domain.Entities;
using Messenger.Domain.Filters;

namespace Messenger.Domain.Interfaces;

public interface IUserChatRepository : IBaseRepository<UserChatEntity, UserChatFilter>
{
}