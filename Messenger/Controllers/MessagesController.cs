using System.Security.Claims;
using AutoMapper;
using Messenger.Dtos.MessageDtos;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using Messenger.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/chat")]
[ApiController]
public class MessagesController : Controller
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public MessagesController(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("getMessagesListInChat")]
    public async Task<ActionResult<List<GetMessageOutDto>>> GetAllMessagesInChatAsync([FromQuery] int chatId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(user, out var userId))
        {
            return BadRequest();
        }
        else
        {
            return Ok(await _messageService.GetAllMessagesAsync(userId, chatId));
        }
    }
    
    [HttpGet("getMessageById")]
    public async Task<ActionResult<MessageModel>> GetMessageInChatById([FromQuery] int messageId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(user, out var userId))
        {
            return BadRequest();
        }
        else
        {
            return Ok(await _messageService.GetMessageAsync(messageId, userId));
        }
    }

    [Authorize]
    [HttpPost("createMessage")]
    public async Task<ActionResult> CreateMessageAsync([FromBody] CreateMessageDto createMessage)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (createMessage.Text.Length > 256 || createMessage.Text == string.Empty)
        {
            return BadRequest();
        }

        if (int.TryParse(user, out var userId))
        {
            var messageModel = _mapper.Map<MessageModel>(createMessage);
            messageModel.UserId = userId;
            messageModel.CreatedDate = DateTime.UtcNow;
            messageModel.IsPinned = false;
            await _messageService.CreateMessageAsync(messageModel, userId);

            return Ok("Created");
        }
        else
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpPatch("modifyMessage")]
    public async Task<ActionResult> ModifyMessageAsync(ModifyMessageDto modifyMessage)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(user, out var userId))
        {
            return BadRequest();
        }

        await _messageService.ModifyMessageAsync(modifyMessage.MessageId, modifyMessage.Text, userId);

        return Ok();
    }

    [HttpDelete("deleteMessage")]
    public async Task<ActionResult> DeleteMessage(int messageId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(user, out var userId))
        {
            return BadRequest();
        }
        else
        {
            await _messageService.RemoveMessageAsync(messageId, userId);

            return Ok();
        }
    }
}