using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IAuthService
{
    Task<UserAuthModel?> RegisterAsync(UserAuthModel userAuthModel);
    Task<string> LoginAsync(UserAuthModel userAuthModel);
}