using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class MessageRepository : BaseRepository<MessageEntity, MessageFilter>, IMessageRepository
{
    public MessageRepository(DataContext context) : base(context) { }

    protected override IQueryable<MessageEntity> ApplyFilter(IQueryable<MessageEntity> query, MessageFilter filter)
    {
        var result = query;

        if (filter is { ChatId: not null, UserId: not null })
        {
            result = result.Where(message => message.Chat.UserChats
                .Any(uc => uc.ChatId == filter.ChatId
                    && uc.UserId == filter.UserId));
        }

        if (filter.MessageId is not null)
        {
            result = result.Where(message => message.Id == filter.MessageId);
        }

        if (filter.UserId is not null)
        {
            result = result.Where(message => message.Chat.UserChats.Any(uc => uc.UserId == filter.UserId));
        }

        if (!string.IsNullOrWhiteSpace(filter.MessageText) && filter.UserId is not null)
        {
            result = result.Where(message => message.Text.Contains(filter.MessageText));
        }

        return result;
    }
}