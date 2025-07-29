using Messenger.Dtos.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/chats")]
[ApiController]
public class ChatsController : Controller
{
    [HttpPost]
    public ActionResult CreateChat(CreateChatDto createChat)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public ActionResult GetAllChats([FromQuery]int userId)
    {
        return Ok("list ur chats");
    }

    [Authorize]
    [HttpGet("test")]
    public ActionResult<string> TestChat()
    {
        return Ok("u was authorize");
    }

    [HttpGet("Search")]
    //уточнение должно быть сравнение по совпадениям через базу
    public ActionResult SearchChat([FromQuery] string contactName)
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    public ActionResult ChangeChat(ChangeChatDto changeChat)
    {
        if (changeChat.Id == 0)
        {
            return BadRequest();
        }

        if (changeChat.Name == null && changeChat.IsPublic == null && changeChat.Description == null)
        {
            return BadRequest();
        }

        throw new NotImplementedException();
    }

    [HttpDelete]
    public ActionResult DeleteChat(int chatId)
    {
        if (chatId == 0)
        {
            return BadRequest();
        }

        throw new NotImplementedException();
    }
}