using Messenger.Domain;
using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(string userEmail);

    Task<UserModel?> FindUserAsync(string searchRequest, SearchType searchType);

    Task UpdateUserAsync(UserModel userEntity);

    Task AddContactAsync(int ownerUserId, int contactUserId, string displayName);

    Task DeleteContactAsync(int ownerId, int contactId);
}