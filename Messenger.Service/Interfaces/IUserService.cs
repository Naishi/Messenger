using Messenger.Domain;
using Messenger.Domain.Entities;
using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(string userEmail);
    Task<UserModel?> FindUserAsync(string searchRequest, SearchType searchType);
    Task UpdateUserAsync(UserModel userEntity, string searchRequest, SearchType searchType);
    Task<List<UserModel>?> GetUsersAsync();
}