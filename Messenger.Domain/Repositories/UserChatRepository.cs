using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserChatRepository : IUserChatRepository
{
    private readonly DataContext _context;

    public UserChatRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<UserChatEntity?> GetUserChatAsync(int userId, int chatId)
    {
        var userChatEntity = await _context.UserChats.FirstOrDefaultAsync();
        return userChatEntity ?? null;
    }

    public async Task AddUserChatAsync(UserChatEntity userChatEntity)
    {
        await _context.UserChats.AddAsync(userChatEntity);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserChatEntityListAsync(List<UserChatEntity> userChatEntities)
    {
        await _context.UserChats.AddRangeAsync(userChatEntities);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserChatEntity>> GetListUserChatsAsync(int chatid)
    {
        var list = await _context.UserChats.Where(chat => chat.ChatId == chatid).ToListAsync();

        return list;
    }

    public async Task<bool> CheckExistingChatAsync(int userId1, int userId2)
    {
        return await _context.UserChats
            .GroupBy(uc => uc.ChatId)
            .AnyAsync(g => g.Any(uc => uc.UserId == userId1
                && g.Any(uc => uc.UserId == userId2)));
    }

    public async Task<List<ChatEntity>> GetChatsByUserIdAsync(int userId)
    {
        var chats = await _context.UserChats
            .Where(uc => uc.UserId == userId)
            .Select(uc => uc.Chat)
            .ToListAsync();
        return chats;
    }

    public async Task DeleteUserChatAsync(UserChatEntity userChatEntity)
    {
        _context.UserChats.Remove(userChatEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserEntity>?> GetUsersByChatIdAsync(int chatId)
    {
        var users = await _context.UserChats.Where(uc => uc.ChatId == chatId).Include(uc => uc.User).Select(uc => uc.User).ToListAsync();

        if (users.Count == 0)
        {
            return null;
        }
        return users;
    }
}