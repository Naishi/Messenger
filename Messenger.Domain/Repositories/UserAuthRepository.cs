using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserAuthRepository : BaseRepository<UserAuthEntity, AuthFilter>, IUserAuthRepository
{
    public UserAuthRepository(DataContext context) : base(context) { }

    protected override IQueryable<UserAuthEntity> ApplyFilter(IQueryable<UserAuthEntity> query, AuthFilter filter)
    {
        var result = query;

        if (filter.Id != null)
        {
            result = result.Where(a => a.Id == filter.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            result = result.Where(a => a.Email ==  filter.Search);
        }
        return result;
    }
}