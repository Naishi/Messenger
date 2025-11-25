using System.Security.Claims;

using AutoMapper;

using Messenger.Domain.Interfaces;
using Messenger.Dtos.MessageDtos;
using Messenger.Service.Models;
using Messenger.Service.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/chat")]
[ApiController]
public class MessagesController : Controller
{
    private readonly IMessageService _messageService;
    private readonly IMapper  _mapper;

    public MessagesController(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }
    /// <summary>
    /// 
    /// </summary>
    [HttpGet]
    public ActionResult<List<GetMessageOutDto>> GetAllMessageInChat([FromQuery] int chatId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("search")]
    public ActionResult<List<GetMessageOutDto>> GetMessageInChat([FromQuery] int userChatId, [FromQuery] string text)
    {
        throw new NotImplementedException();
    }

    [HttpGet("fullsearch")]
    public ActionResult<List<GetMessageOutDto>> GetMessageInAllChats([FromHeader] int userId, [FromQuery] string text)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateMessageAsync([FromBody] CreateMessageDto createMessage)
    {
        var user =  User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (createMessage.Text.Length > 256 || createMessage.Text == string.Empty)
        {
            return BadRequest();
        }

        if ( int.TryParse(user, out var userId))
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

    [HttpPatch]
    public ActionResult ModifyMessage(ModifyMessageDto modifyMessage)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public ActionResult DeleteMessage(int chatId)
    {
        throw new NotImplementedException();
    }
}