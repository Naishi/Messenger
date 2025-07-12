using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(UserRegisterModel model);
    Task<string> LoginAsync(UserLoginModel model);
}