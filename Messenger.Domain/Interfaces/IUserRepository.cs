using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity user);
    Task<bool> DeleteUserAsync(string userEmail);
    Task UpdateUserInfoAsync();
    Task<UserEntity?> FindUserByCriteriaAsync(string searchRequest, SearchType searchType);
    Task<List<UserEntity>?> GetUsersAsync();
}