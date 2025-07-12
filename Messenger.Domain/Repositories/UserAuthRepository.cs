using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserAuthRepository : IUserAuthRepository
{
    private readonly DataContext _context;

    public UserAuthRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<UserAuthEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.UserAuth.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task RegisterUserAsync(UserAuthEntity user)
    {
        if (!await _context.UserAuth.AnyAsync(u => u.Email == user.Email))
        {
            await _context.UserAuth.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}