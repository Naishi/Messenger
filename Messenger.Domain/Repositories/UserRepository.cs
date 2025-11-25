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

        if (string.IsNullOrWhiteSpace(cleanedEmail))
            return false;

        if (userEntity == null)
            return false;
        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();

        return true;
    }

    public Task UpdateUserInfoAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> FindUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    private async Task<UserEntity?> FindUserByIdAsync(string userId)
    {
        var id = int.Parse(userId);

        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserEntity?> FindUserByCriteriaAsync(string searchRequest, SearchType searchType) =>
        searchType switch
        {
            SearchType.Id => await FindUserByIdAsync(searchRequest),
            SearchType.Email => await _context.Users.FirstOrDefaultAsync(u => u.Email == searchRequest),
            SearchType.Nickname => await _context.Users.FirstOrDefaultAsync(u => u.NickName == searchRequest),
            _ => null
        };

    public async Task<bool> RoleCheckAsync(UserEntity verifyUser)
    {
        var authEntity = await _context.UserAuth.FirstOrDefaultAsync(u => u.Email == verifyUser.Email);

        return authEntity != null;
    }

    public async Task<List<UserEntity>?> GetUsersAsync()
    {
        var query = from user in _context.Users
                    join authUser in _context.UserAuth
                        on user.Email equals authUser.Email
                    where authUser.Role != "Admin"
                    select user;

        /*var query1 = _context.Users
            .AsNoTracking()
            .Join(_context.UserAuth, x => x.Email, y => y.Email)
            .Where()*/
        return await query
            .OrderBy(u => u.Email)
            .ToListAsync();
    }

    public async Task<bool> CheckUserExistAsync(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> CheckUsersExistAsync(List<int> userIds)
    {
        return await _context.Users.AnyAsync(u => userIds.Contains(u.Id));
    }
    public async Task AddContactAsync(ContactEntity contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    public async Task<ContactEntity?> FindContactByIdAsync(int ownerId, int contactId)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.OwnerUserId == ownerId && c.ContactUserId == contactId);
    }

    public async Task DeleteContactAsync(ContactEntity contact)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task<ContactEntity?> GetContactAsync(int contactId)
    {
        if (contactId <= 0)
            return null;
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);

        return contact;
    }

    public async Task<bool> CheckContactExistAsync(int contactId)
    {
        return await _context.Contacts.AnyAsync(c => c.Id == contactId);
    }

    public async Task<bool> CheckContactThisUserExistAsync(int ownerId, int contactId)
    {
        return await _context.Contacts
            .AnyAsync(contact => contact.OwnerUserId == ownerId && contact.ContactUserId == contactId);
    }
}