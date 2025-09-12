using AutoMapper;
using Messenger.Dtos;
using Messenger.Dtos.UserDtos;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Messenger.Service.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody]RegisterRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var authModel = _mapper.Map<UserAuthRegisterModel>(request);
            authModel.Role = UserRole.Client;
            var userModel = _mapper.Map<UserModel>(request);
            if(!await _authService.RegisterAsync(authModel, userModel))
            {
                return BadRequest("не удалось зарегистрировать пользователя");
            }
            return Created();
        }
        catch (UndefinedUserRoleException)
        {
            return Conflict();
        }
        catch (ExistedUserException)
        {
            return Conflict();
        }
        catch (BirthDateException)
        {
            return BadRequest("Need real birthday");
        }
        catch (InvalidEmailException)
        {
            return BadRequest("Need real email");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseModel>> Login([FromBody]UserAuthDto request)
    {
        try
        {
            var model = _mapper.Map<UserAuthLoginModel>(request);
            var tokenResponseModel = await _authService.LoginAsync(model);
            return Ok(tokenResponseModel);
        }
        catch (UserNotFoundException)
        {
            return Conflict();
        }
        catch (UserLoginException)
        {
            return Conflict();
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponseModel>> RefreshToken([FromBody] RefreshTokenRequestModel request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if(result?.AccessToken is null || result?.RefreshToken is null)
            return Unauthorized("Invalid token");
        return Ok(result);
    }
}