using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserChatRepository : BaseRepository<UserChatEntity, UserChatFilter>,  IUserChatRepository
{
    public UserChatRepository(DataContext context) : base(context) { }

    protected override IQueryable<UserChatEntity> ApplyFilter(IQueryable<UserChatEntity> query, UserChatFilter filter)
    {
        var result = query;

        if (filter.UserIds.Count != 0 && filter.ChatIds.Count != 0)
        {
            result = result.Where(uc => filter.ChatIds.Contains(uc.ChatId) &&  filter.UserIds.Contains(uc.UserId));
        }

        if (filter.UserChatIds.Count != 0)
        {
            result = result.Where(uc => filter.UserChatIds.Contains(uc.Id));
        }

        if (filter.ChatIds.Count != 0)
        {
            result = result.Where(uc => filter.ChatIds.Contains(uc.ChatId));
        }
        
        return result;
    }
}