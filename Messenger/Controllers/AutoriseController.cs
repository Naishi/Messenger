using AutoMapper;
using Messenger.Dtos;
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
    public async Task<ActionResult> Register([FromBody]UserAuthDto request)
    {
        try
        {
            var model = _mapper.Map<UserRegisterModel>(request);
            model.Role = UserRole.Client;
            await _authService.RegisterAsync(model);
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
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody]UserAuthDto request)
    {
        try
        {
            var model = _mapper.Map<UserLoginModel>(request);
            var token = await _authService.LoginAsync(model);
            return Ok(token);
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
}