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

    public async Task RegisterAsync(UserRegisterModel model)
    {
        if (model.Role == UserRole.None)
        {
            throw new UndefinedUserRoleException();
        }

        if (await _userAuthRepository.GetUserByEmailAsync(model.Email) != null)
        {
            throw new ExistedUserException();
        }

        var entity = _mapper.Map<UserAuthEntity>(model);
        entity.PasswordHash = _passwordHasher.HashPassword(entity, model.Password);

        await _userAuthRepository.RegisterUserAsync(entity);
    }

    public async Task<string> LoginAsync(UserLoginModel model)
    {
        var entity = await _userAuthRepository.GetUserByEmailAsync(model.Email);
        if (entity == null)
        {
            throw new UserNotFoundException();
        }

        var result = _passwordHasher.VerifyHashedPassword(entity, entity.PasswordHash, model.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UserLoginException();
        }

        entity = await _userAuthRepository.GetUserByEmailAsync(model.Email);
        var userInfo = _mapper.Map<UserAuthModel>(entity);
        return CreateToken(userInfo);
    }

    private string CreateToken(UserAuthModel user)
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
}