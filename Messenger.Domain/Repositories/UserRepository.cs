using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUserAsync(UserEntity user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string userEmail)
    {
        var cleanedEmail = userEmail.Trim().ToLower();
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == cleanedEmail);
        if(string.IsNullOrWhiteSpace(cleanedEmail))
            return false;
        
        if (userEntity == null)
            return false;
        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateUserInfoAsync()
    {
        await _context.SaveChangesAsync();
    }

    private async Task<UserEntity?> FindByIdAsync(string userId)
    {
        var id = int.Parse(userId);
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserEntity?> FindUserByCriteriaAsync(string searchRequest, SearchType searchType)
    {
        return searchType switch
        {
            SearchType.Id => await FindByIdAsync(searchRequest),
            SearchType.Email => await _context.Users.FirstOrDefaultAsync(u => u.Email == searchRequest),
            SearchType.Nickname => await _context.Users.FirstOrDefaultAsync(u => u.NickName == searchRequest),
            _ => null
        };
    }

    public async Task<List<UserEntity>?> GetUsersAsync()
    {
        var query = from user in _context.Users
            join authUser in _context.UserAuth 
                on user.Email equals authUser.Email
                where authUser.Role != "Admin"
                select user;

        if (!query.Any())
            return null;
        
        return await query
            .OrderBy(u => u.Email)
            .ToListAsync();
    }
}