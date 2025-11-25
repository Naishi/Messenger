using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity user);
    Task<bool> DeleteUserAsync(string userEmail);
    Task UpdateUserInfoAsync();
    Task<UserEntity?> FindUserByIdAsync(int userId);
    Task<UserEntity?> FindUserByCriteriaAsync(string searchRequest, SearchType searchType);
    Task<List<UserEntity>?> GetUsersAsync();
    Task<bool> CheckUserExistAsync(int id);
    Task AddContactAsync(ContactEntity contact);
    Task DeleteContactAsync(ContactEntity contact);
    Task<ContactEntity?> GetContactAsync(int contactId);
    Task<ContactEntity?> FindContactByIdAsync(int ownerId, int contactId);
    Task<bool> RoleCheckAsync(UserEntity verifyUser);
    Task<bool> CheckUsersExistAsync(List<int> userIds);
    Task<bool> CheckContactExistAsync(int contactId);

    Task<bool> CheckContactThisUserExistAsync(int ownerId, int contactId);
}