using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserAuthRepository
{
    private readonly DataContext _context;

    public UserAuthRepository(DataContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// user = userAuth obj from request body or from auth service for registration
    /// using in login method for validate data
    /// </summary>

    public async Task<UserAuthEntity?> GetUserByEmailAsync(UserAuthEntity user)
    {
        var userAuth = await _context.UserAuth.AnyAsync
            (u => u.Email == user.Email);
        if (userAuth)
        {
            return await _context.UserAuth.FirstOrDefaultAsync(u => u.Email == user.Email);
        }
        return null;
    }
    
    
    
    /// <summary>
    /// userAuth obj from request body or from auth service for registration
    /// 
    /// </summary>

    public async Task<UserAuthEntity?> RegisterUserAsync(UserAuthEntity user)
    {
        if (await _context.UserAuth.AnyAsync
                (u => u.Email == user.Email))
        {
            return null;
        }
        await _context.UserAuth.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}