using System.Security.Claims;
using Messenger.Dtos.ChatDtos;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/chats")]
[ApiController]
public class ChatsController : Controller
{
    private readonly IChatService _chatService;

    public ChatsController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize]
    [HttpPost("addChats")]
    public async Task<ActionResult> CreateChat(CreateChatDto createChat)
    {
        try
        {
            var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chatModel = new ChatModel()
            {
                Name = createChat.ChatName, CreatedDate = DateOnly.FromDateTime(DateTime.Today)
            };

            if (string.IsNullOrWhiteSpace(ownerUserId) || !int.TryParse(ownerUserId, out var ownerId))
            {
                return Unauthorized("invalid or missing user identifier");
            }

            var usersList = new List<int>
            {
                ownerId, createChat.InvitedUserId
            };

            await _chatService.AddChatAsync(chatModel, usersList);

            return Ok("Chat created successfully");
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
        catch (ExistedChatException)
        {
            return Conflict();
        }
        catch (ForbiddenOperationExceprion)
        {
            return Forbid();
        }
        catch (ChatNameExistedException)
        {
            return Conflict();
        }
    }

    [Authorize]
    [HttpGet("Search")]
    
    public ActionResult SearchChat([FromQuery] string contactName)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("SearchByCriteria")]
    public async Task<ActionResult<SearchChatDto>> SearchByCriteria([FromQuery] string contactName)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(ownerId, out var id))
        {
            return Ok(await _chatService.SearchChatsByCriteriaAsync(contactName, id));
        }
        else
        {
            return Unauthorized("invalid or missing user identifier");
        }
    }

    [Authorize]
    [HttpDelete("DeleteChat")]
    public async Task<ActionResult> DeleteChat([FromQuery]int chatId)
    {
        try
        {
            var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (chatId <= 0)
            {
                return BadRequest();
            }

            if (int.TryParse(ownerUserId, out var ownerId))
            {
                await _chatService.DeleteChatAsync(chatId, ownerId);

                return Ok("Chat deleted successfully");
            }

            return Unauthorized();
        }
        catch (ChatNotFoundException)
        {
            return BadRequest();
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpGet("GetChats")]
    public async Task<ActionResult<List<ChatBasicModel>>> GetChatsAsync()
    {
        var ownerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ownerUserId == null || !int.TryParse(ownerUserId, out var ownerId))
        {
            return Unauthorized("invalid or missing user identifier");
        }

        return Ok(await _chatService.GetChatsAsync(ownerId));
    }
}