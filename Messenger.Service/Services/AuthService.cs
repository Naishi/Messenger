using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Messenger.Service.Models.Enums;
using Messenger.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Messenger.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<UserAuthEntity> _passwordHasher;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserAuthRepository user,
        IMapper mapper,
        IPasswordHasher<UserAuthEntity> passwordHasher,
        JwtSettings jwtSettings)
    {
        _userAuthRepository = user;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtSettings;
    }

    public async Task RegisterAsync(UserAuthRegisterModel authModel, UserModel userModel)
    {
        if (authModel.Role == UserRole.None)
        {
            throw new UndefinedUserRoleException();
        }

        if (await _userAuthRepository.GetUserAsync(authModel.Email) != null)
        {
            throw new ExistedUserException();
        }

        if (userModel.Birthday == DateOnly.FromDateTime(DateTime.Now))
        {
            throw new BirthDateException();
        }

        var authEntity = _mapper.Map<UserAuthEntity>(authModel);
        authEntity.PasswordHash = _passwordHasher.HashPassword(authEntity, authModel.Password);
        
        var userEntity = _mapper.Map<UserEntity>(userModel);

        await _userAuthRepository.RegisterUserAsync(authEntity,userEntity);
    }

    public async Task<TokenResponseModel?> LoginAsync(UserAuthLoginModel model)
    {
        var entity = await _userAuthRepository.GetUserAsync(model.Email);
        if (entity == null)
        {
            throw new UserNotFoundException();
        }

        var result = _passwordHasher.VerifyHashedPassword(entity, entity.PasswordHash, model.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UserLoginException();
        }
        
        return await CreateTokenResponse(entity);
    }

    private async Task<TokenResponseModel> CreateTokenResponse( UserAuthEntity entity)
    {
        var response = new TokenResponseModel()
        {
            AccessToken = CreateToken(entity),
            RefreshToken = await GenerateAndSaveRefreshToken(entity)
        };
        return response;
    }

    public async Task<TokenResponseModel?> RefreshTokenAsync(RefreshTokenRequestModel model)
    {
        var user = await ValidateRefreshTokenAsync(model);
        if(user is null)
            return null;
        return await CreateTokenResponse(user);
    }

    private async Task<UserAuthEntity?> ValidateRefreshTokenAsync(RefreshTokenRequestModel model)
    {
        var user = await _userAuthRepository.GetUserAsync(model.Id);
        if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    private string CreateToken(UserAuthEntity user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Token));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshToken(UserAuthEntity userAuth)
    {
        var refreshToken = GenerateRefreshToken();
        
        userAuth!.RefreshToken = refreshToken;
        userAuth.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1);
        await _userAuthRepository.SaveRefreshTokenAsync();
        return refreshToken;
    }
    
}