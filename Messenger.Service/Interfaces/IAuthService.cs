using Messenger.Service.Models;

namespace Messenger.Service.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(UserAuthRegisterModel authModel, UserModel user);

    Task<TokenResponseModel?> LoginAsync(UserAuthLoginModel model);

    Task<TokenResponseModel?> RefreshTokenAsync(RefreshTokenRequestModel model);
}