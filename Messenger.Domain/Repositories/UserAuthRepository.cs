using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserAuthRepository : IUserAuthRepository
{
    private readonly DataContext _context;
    private readonly UserRepository _userRepository;

    public UserAuthRepository(DataContext context, UserRepository userRepository)
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

    public async Task RegisterUserAsync(UserAuthEntity userAuth, UserEntity user)
    {
        if (!await _context.UserAuth.AnyAsync(u => u.Email == userAuth.Email) 
            && !await _context.Users.AnyAsync((u => u.NickName == user.NickName)))
        {
            await _context.UserAuth.AddAsync(userAuth);
            await _userRepository.AddUser(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveRefreshTokenAsync()
    {
        await _context.SaveChangesAsync();
    }
}