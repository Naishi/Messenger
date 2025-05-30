using Messenger.Dtos.ChatDtos;
using Messenger.Service.Services;
using Microsoft.AspNetCore.Http;
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
    public ActionResult GetAllChats(int userId)
    {
        throw new NotImplementedException();
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