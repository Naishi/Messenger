using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<UserAuthEntity?> GetUserAsync(string email);
        Task<UserAuthEntity?> GetUserAsync(int id);
        Task<bool> RegisterUserAsync(UserAuthEntity userAuth, UserEntity user);
        Task SaveRefreshTokenAsync();
    }
}