using System.ComponentModel;

using Messenger.Domain.Data;
using Messenger.Domain.Dto;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly DataContext _context;

    public ChatRepository(DataContext context)
    {
        _context = context;
    }

    public async Task CreateChatAsync(ChatEntity chat)
    {
        await _context.Chats.AddAsync(chat);
        await _context.UserChats.AddRangeAsync(chat.UserChats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateChatAsync(ChatEntity chat)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteChatAsync(int id)
    {
        var chat = await _context.Chats.FindAsync(id);

        if (chat != null)
        {
            _context.Chats.Remove(chat);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ChatEntity?> GetChatAsync(int id)
    {
        var find = await _context.Chats.FindAsync(id);

        return find ?? null;
    }

    public async Task<List<UserChatEntity>> GetAllChatsAsync(int userId)
    {
        return await _context.UserChats
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Chat)
            .ToListAsync();
    }

    public async Task<List<SearchChatEntity>> SearchChatByCriteriaAsync(int currentUserId, string searchTerm)
    {
        var term = searchTerm.ToLower();

        var results = await _context.UserChats
            .Where(uc => uc.UserId == currentUserId)
            .Select(uc => new
            {
                BaseChat = uc.Chat,
                Interlocutor = uc.Chat.UserChats
                    .FirstOrDefault(otherUc => otherUc.UserId != currentUserId)
            })
            .Select(temp1 => new
            {
                BaseChat = temp1.BaseChat,
                InterlocutorUser = temp1.Interlocutor!.User,
                CustomContact = _context.Contacts
                    .FirstOrDefault(c =>
                        c.OwnerUserId == currentUserId && c.ContactUserId == temp1.Interlocutor.UserId)
            })
            .Where(temp2 =>
                (temp2.CustomContact != null
                    && temp2.CustomContact.DisplayName != null
                    && temp2.CustomContact.DisplayName.ToLower().Contains(term))
                || temp2.InterlocutorUser.Name.ToLower().Contains(term)
                || temp2.InterlocutorUser.NickName != null && temp2.InterlocutorUser.NickName.ToLower().Contains(term))
            .Select(finalData => new SearchChatEntity
            {
                Id = finalData.BaseChat.Id,
                CreatedDate = finalData.BaseChat.CreatedDate,
                DisplayName = finalData.CustomContact!.DisplayName
                    ?? finalData.InterlocutorUser.NickName
                    ?? finalData.InterlocutorUser.Name,
            })
            .ToListAsync();

        return results;
    }
}