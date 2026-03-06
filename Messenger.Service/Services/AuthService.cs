using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Messenger.Service.Models.Enums;
using Messenger.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<UserAuthEntity> _passwordHasher;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailValidator _emailValidator;

    public AuthService(
        IUserAuthRepository userAuth,
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher<UserAuthEntity> passwordHasher,
        JwtSettings jwtSettings,
        IEmailValidator emailValidator)
    {
        _userAuthRepository = userAuth;
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtSettings;
        _emailValidator = emailValidator;
    }
    
    public async Task RegisterAsync(UserAuthRegisterModel authModel, UserModel userModel)
    {
        if (!await _emailValidator.IsValidEmail(authModel.Email))
        {
            throw new InvalidEmailException();
        }

        if (authModel.Role == UserRole.None)
        {
            throw new UndefinedUserRoleException();
        }

        UserFilter userFilter = new UserFilter()
        {
            Search = userModel.NickName
        };

        AuthFilter authFilter = new AuthFilter()
        {
            Search = authModel.Email
        };

        var getAuthByEmail = await _userAuthRepository.GetAsync(authFilter);
        var getUserByNickName = await _userRepository.GetAsync(userFilter);

        if (getAuthByEmail.Any() || getUserByNickName.Any())
        {
            throw new ExistedUserException();
        }

        if (userModel.Birthday >= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new BirthDateException();
        }

        var authEntity = _mapper.Map<UserAuthEntity>(authModel);
        authEntity.PasswordHash = _passwordHasher.HashPassword(authEntity, authModel.Password);

        var userEntity = _mapper.Map<UserEntity>(userModel);

        await using var transaction = await _userAuthRepository.BeginTransactionAsync();

        try
        {
            await _userAuthRepository.CreateAsync(authEntity);
            await _userRepository.CreateAsync(userEntity);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    public async Task<TokenResponseModel?> LoginAsync(UserAuthLoginModel model)
    {
        AuthFilter authFilter = new AuthFilter()
        {
            Search = model.Email
        };
        
        var entity1 = await _userAuthRepository.GetAsync(authFilter);
        var entity = entity1.SingleOrDefault();

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

    private async Task<TokenResponseModel> CreateTokenResponse(UserAuthEntity entity)
    {
        var response = new TokenResponseModel()
        {
            AccessToken = CreateToken(entity), RefreshToken = await GenerateAndSaveRefreshToken(entity)
        };

        return response;
    }

    public async Task<TokenResponseModel?> RefreshTokenAsync(RefreshTokenRequestModel model)
    {
        var user = await ValidateRefreshTokenAsync(model);

        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }

    private async Task<UserAuthEntity?> ValidateRefreshTokenAsync(RefreshTokenRequestModel model)
    {
        AuthFilter authFilter = new()
        {
            Id = model.Id
        };
        var users = await _userAuthRepository.GetAsync(authFilter);
        var user = users.SingleOrDefault();

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
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Token));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshToken(UserAuthEntity userAuth)
    {
        var refreshToken = GenerateRefreshToken();

        userAuth.RefreshToken = refreshToken;
        userAuth.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1);
        await _userAuthRepository.UpdateAsync(userAuth);

        return refreshToken;
    }
}