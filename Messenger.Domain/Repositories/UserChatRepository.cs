using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserChatRepository
{
    private readonly DataContext _context;

    public UserChatRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<UserChatEntity?> GetUserChatEntity(int userId, int chatId)
    {
        var userChatEntity = await _context.UserChats.FirstOrDefaultAsync();
        return userChatEntity ?? null;
    }
}