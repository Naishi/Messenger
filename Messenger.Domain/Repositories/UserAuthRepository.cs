using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserAuthRepository : IUserAuthRepository
{
    private readonly DataContext _context;
    private readonly IUserRepository _userRepository;

    public UserAuthRepository(DataContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public async Task<UserAuthEntity?> GetUserAsync(string email)
    {
        return await _context.UserAuth.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserAuthEntity?> GetUserAsync(int id)
    {
        return await _context.UserAuth.FindAsync(id);
    }

    public async Task<bool> RegisterUserAsync(UserAuthEntity userAuth, UserEntity user)
    {
        if (await _context.UserAuth.AnyAsync(u => u.Email == userAuth.Email)
            || !await IsNickUniqueAsync(user.NickName)) return false;
        await _context.UserAuth.AddAsync(userAuth);
        if (!await _userRepository.AddUserAsync(user))
        {
            return false;
        }
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> IsNickUniqueAsync(string nickName)
    {
        if (string.IsNullOrWhiteSpace(nickName))
        {
            return true;
        }
        var exist = await _context.Users.AnyAsync(u => u.NickName == nickName);
        return !exist;
    }

    public async Task SaveRefreshTokenAsync()
    {
        await _context.SaveChangesAsync();
    }
}