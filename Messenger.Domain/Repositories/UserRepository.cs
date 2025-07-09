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

    public void AddUser(UserEntity user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(UserEntity user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void UpdateUserInfo(UserEntity user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}