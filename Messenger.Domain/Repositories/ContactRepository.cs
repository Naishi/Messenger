using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class ContactRepository : BaseRepository<ContactEntity, ContactFilter>, IContactRepository
{
    private readonly IUserRepository _userRepository;
    
    public ContactRepository(DataContext context, IUserRepository userRepository) : base(context)
    {
        _userRepository = userRepository;
    }

    protected override IQueryable<ContactEntity> ApplyFilter(IQueryable<ContactEntity> query, ContactFilter filter)
    {
        var result = query;

        if (filter is { OwnerUserId: not null, ContactUserId: not null} 
            and {OwnerUserId: > 0, ContactUserId: > 0})
        {
            result = result.Where(c => 
                (c.OwnerUserId == filter.OwnerUserId && c.ContactUserId == filter.ContactUserId)
            || (c.OwnerUserId == filter.ContactUserId && c.ContactUserId == filter.OwnerUserId));
        }

        if (!string.IsNullOrEmpty(filter.DisplayName) && filter.OwnerUserId == null)
        {
            result = result.Where(c => c.DisplayName.Contains(filter.DisplayName));
        }

        if (!string.IsNullOrEmpty(filter.DisplayName) && filter.OwnerUserId != null)
        {
            result = result.Where(c => c.OwnerUserId == filter.OwnerUserId 
                && c.DisplayName.Contains(filter.DisplayName));
        }
        
        return result;
    }
}
