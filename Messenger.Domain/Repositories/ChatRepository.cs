using Messenger.Domain.Data;
using Messenger.Domain.Entities;

namespace Messenger.Domain.Repositories;

public class ChatRepository
{
    private readonly DataContext _context;

    public ChatRepository(DataContext context)
    {
        _context = context;
    }

    public void CreateChat(ChatEntity chat)
    {
        _context.Chats.Add(chat);
        _context.SaveChanges();
    }

    public void UpdateChat(ChatEntity chat)
    {
        _context.Chats.Update(chat);
        _context.SaveChanges();
    }

    public void DeleteChat(int id)
    {
        var chat = _context.Chats.Find(id);
        if (chat != null)
        {
            _context.Chats.Remove(chat);
        }
        _context.SaveChanges();
    }

    public ChatEntity GetChat(int id)
    {
        return _context.Chats.Find(id);
    }

    public List<ChatEntity> GetAllChats()
    {
        return _context.Chats.ToList();
    }
}