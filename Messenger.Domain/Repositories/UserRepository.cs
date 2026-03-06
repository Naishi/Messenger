using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserRepository : BaseRepository<UserEntity, UserFilter>, IUserRepository
{
    public UserRepository(DataContext context) : base(context) { }

    protected override IQueryable<UserEntity> ApplyFilter(IQueryable<UserEntity> query, UserFilter filter)
    {
        var result = query;

        if (filter.UserIds.Count != 0)
        {
            result = result.Where(u => filter.UserIds.Contains(u.Id));
        }

        if (!string.IsNullOrEmpty(filter.Search) && filter.SearchType != SearchType.None)
        {
            result = filter.SearchType switch
            {
                SearchType.Email => result.Where(u => u.Email == filter.Search),
                SearchType.NickName => result.Where(u => u.NickName == filter.Search),
                _ => result
            };
        }

        if (filter.ChatId != null)
        {
            result = result.Where(user => user.UserChats.Any(chat => chat.ChatId == filter.ChatId));
        }

        return result;
    }
}