using Messenger.Domain.Data;
using Messenger.Domain.Dto;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class ChatRepository : BaseRepository<ChatEntity, ChatFilter>, IChatRepository
{
    public ChatRepository(DataContext context) :  base(context) { }
    
    protected override IQueryable<ChatEntity> ApplyFilter(IQueryable<ChatEntity> query, ChatFilter filter)
    {
        var result = query;

        if (filter.ChatIds != null && filter.ChatIds.Any())
        {
            result = result.Where(c => filter.ChatIds.Contains(c.Id));
        }

        if (filter.UserIds != null && filter.UserIds.Any())
        {
            result = result.Where(chat => chat.UserChats.Any(uc => filter.UserIds.Contains(uc.UserId)));
        }

        // поиск по имени контакта 
        if (filter.UserIds is { Count: > 0 } && !string.IsNullOrWhiteSpace(filter.Search))
        {
            result = result.Where(chat =>
                chat.UserChats.Any(ucOwner =>
                    filter.UserIds.Contains(ucOwner.UserId) && 
                    chat.UserChats.Any(ucOther =>
                        ucOther.UserId != ucOwner.UserId && 
                        ucOwner.User.Contacts.Any(contact =>
                                contact.ContactUserId == ucOther.UserId &&
                                contact.DisplayName != null &&
                                contact.DisplayName.Contains(filter.Search)
                        )
                    )
                )
            );
        }
        return result;
    }
}