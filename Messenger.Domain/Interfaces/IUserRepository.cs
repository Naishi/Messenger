using Messenger.Domain.Entities;
using Messenger.Domain.Filters;

namespace Messenger.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity, UserFilter>
{
}