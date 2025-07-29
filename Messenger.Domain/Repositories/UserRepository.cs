using Messenger.Domain.Data;
using Messenger.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddUser(UserEntity user)
    {
        if (!await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteUser(UserEntity user)
    {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
    }

    public void UpdateUserInfo(UserEntity user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}