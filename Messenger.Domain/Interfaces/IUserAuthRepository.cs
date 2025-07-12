using Messenger.Domain.Entities;

namespace Messenger.Domain.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<UserAuthEntity?> GetUserByEmailAsync(string email);
        Task RegisterUserAsync(UserAuthEntity user);
    }
}
