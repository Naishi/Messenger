using Messenger.Dtos.MessageDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/chat")]
[ApiController]
public class MessagesController : Controller
{
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
    public ActionResult<List<GetMessageOutDto>> GetMessageInAllChats([FromQuery] int userId, [FromQuery] string text)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public ActionResult CreateMessage([FromBody] CreateMessageDto createMessage)
    {
        throw new NotImplementedException();
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