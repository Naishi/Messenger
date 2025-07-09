using AutoMapper;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Microsoft.AspNetCore.Identity;
using Messenger.Domain.Entities;
using Messenger.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Messenger.Service.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    private UserAuthRepository _user;
    private IMapper _mapper;
    public async Task<UserAuthModel?> RegisterAsync(UserAuthModel modelFromRequest)
    {
        var authEntity = _mapper.Map<UserAuthEntity>(modelFromRequest);
        if (await _user.GetUserByEmailAsync(authEntity)!= null)
        {
            return null;
        }

        var userAuthModel =  new UserAuthModel();
        var hashedPassword = new PasswordHasher<UserAuthModel>()
            .HashPassword(userAuthModel,  modelFromRequest.PasswordHash);
        userAuthModel.Email = modelFromRequest.Email;
        userAuthModel.PasswordHash = hashedPassword;
        var userAuthEntity = _mapper.Map<UserAuthEntity>(userAuthModel);
        await _user.RegisterUserAsync(userAuthEntity);
        return userAuthModel;
    }

    /// <summary>
    /// userMapEntity = UserAuth(modelFromRequest)
    /// userData = get auth from data(can be null)
    /// userMapModel = get a model for hasher in "if"
    /// </summary>
    
    public async Task<string> LoginAsync(UserAuthModel modelFromRequest)
    {
        var userMapEntity = _mapper.Map<UserAuthEntity>(modelFromRequest);
        var userData = await _user.GetUserByEmailAsync(userMapEntity);
        var userMapModel = _mapper.Map<UserAuthModel>(userData);
        
        if (userData == null ||
            new PasswordHasher<UserAuthModel>().VerifyHashedPassword(userMapModel, userData.PasswordHash, modelFromRequest.Password) 
            == PasswordVerificationResult.Failed)
        {
            return "Email or password is incorrect";
        }
        return "success";
    }
    
}