using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IChatRepository _chatRepository;
    private readonly IUserChatRepository _userChatRepository;

    public MessageRepository(
        DataContext context,
        IChatRepository chatRepository,
        IUserChatRepository userChatRepository)
    {
        _context = context;
        _chatRepository = chatRepository;
        _userChatRepository = userChatRepository;
    }

    public async Task CreateMessageAsync(MessageEntity message, int userId)
    {
        var usersId =  _context.UserChats.Where(uc => uc.ChatId == message.ChatId).Select(uc => uc.UserId);
        var user = await _context.Users.FindAsync(userId);
        if (usersId.Any(u => u == userId) && user != null)
        {
            message.AuthorName = user.NickName ?? user.Name;
            await _context.Messages.AddAsync(message);
            await _chatRepository.UpdateLastMessageTime(message.ChatId);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveMessageAsync(int messageId, int userId)
    {
        var message = await _context.Messages.FindAsync(messageId);

        if (message != null)
        {
            var userList = await _userChatRepository.GetUsersByChatIdAsync(message.ChatId);

            if (userList != null && userId > 0 && userList.Any(u => u.Id == userId))
            {
                _context.Messages.Remove(message);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task ModifyMessageAsync(int messageId, string text, int userId)
    {
        var messageEntity = await _context.Messages.FindAsync(messageId);

        if (messageEntity != null && messageEntity.UserId == userId)
        {
            messageEntity.Text = text;
        }
        else
        {
            throw new DbUpdateException();
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<MessageEntity>> GetAllMessageAsync(int chatId, int userId)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .ToListAsync();
    }

    public async Task<MessageEntity?> GetMessageByIdAsync(int messageId, int userId)
    {
        var message = await _context.Messages.FindAsync(messageId);

        if (message != null && message.UserId == userId)
        {
            return message;
        }

        return null;
    }
}