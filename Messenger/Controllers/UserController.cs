using System.Security.Claims;
using AutoMapper;
using Messenger.Domain;
using Messenger.Dtos.UserDtos;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin, Client")]
    [HttpGet("user-info")]
    public async Task<ActionResult<UserInfoDto>> GetUser([FromQuery] string searchRequest)
    {
        if (string.IsNullOrWhiteSpace(searchRequest))
            return BadRequest();

        SearchType searchType;

        if (searchRequest.Contains('@'))
        {
            searchType = SearchType.Email;
        }
        else
        {
            searchType = SearchType.NickName;
        }

        var user = await _userService.FindUserAsync(searchRequest, searchType);

        if (user == null)
            return NotFound();
        var userInfo = _mapper.Map<UserInfoDto>(user);

        return Ok(userInfo);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-user")]
    public async Task<ActionResult> DeleteUser([FromQuery] string email)
    {
        try
        {
            await _userService.DeleteUserAsync(email);

            return Ok("Was Deleted");
        }
        catch (InvalidEmailException)
        {
            return BadRequest("Bad Email");
        }
        catch (UserNotFoundException)
        {
            return NotFound("User with this email not found");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("update-user")]
    public async Task<ActionResult> UpdateUserInfo([FromBody] UserChangingInfoDto request)
    {
        try
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(user, out var id))
            {
                return BadRequest();
            }

            var userModel = _mapper.Map<UserModel>(request);
            userModel.Id = id;

            await _userService.UpdateUserAsync(userModel);

            return Ok("Was Updated");
        }

        catch (ProfanityExistException)
        {
            return BadRequest("dont use profanity text");
        }
        catch (EmptyStringsException)
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpPost("add-contact")]
    public async Task<ActionResult> AddContact([FromBody] AddContactDto request)
    {
        try
        {
            var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ownerUserId == null)
                return BadRequest("Invalid user id");

            if (!int.TryParse(ownerUserId, out var ownerUserIdInt))
                return BadRequest("Invalid owner user id");
            await _userService.AddContactAsync(ownerUserIdInt, request.ContactId, request.DisplayName);

            return Ok();
        }
        catch (InvalidIdException)
        {
            return BadRequest("Need valid id");
        }
        catch (EmptyStringsException)
        {
            return BadRequest("Need valid Id or Name");
        }
        catch (UserNotFoundException)
        {
            return NotFound("One or More Users were not found");
        }
        catch (ExistedUserException)
        {
            return BadRequest("Existed contact");
        }
    }

    [Authorize]
    [HttpDelete("delete-contact")]
    public async Task<ActionResult> DeleteContact([FromQuery] int contactId)
    {
        try
        {
            var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ownerUserId == null)
                return BadRequest("Invalid user id");
            if (!int.TryParse(ownerUserId, out var ownerUserIdInt))
                return BadRequest("Invalid owner user id");
            await _userService.DeleteContactAsync(ownerUserIdInt, contactId);

            return Ok();
        }
        catch (InvalidIdException)
        {
            return BadRequest();
        }
    }
}