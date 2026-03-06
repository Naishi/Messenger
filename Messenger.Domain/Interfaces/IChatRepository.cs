using Messenger.Domain.Dto;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;

namespace Messenger.Domain.Interfaces;


public interface IChatRepository : IBaseRepository<ChatEntity, ChatFilter>
{
}