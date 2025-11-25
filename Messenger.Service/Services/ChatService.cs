using AutoMapper;

using Messenger.Domain.Dto;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Models;
using System.Linq;

namespace Messenger.Service.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;
    private readonly IUserChatRepository _userChatRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(
        IChatRepository chatRepository,
        IMapper mapper,
        IUserChatRepository userChatRepository,
        IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _mapper = mapper;
        _userChatRepository = userChatRepository;
        _userRepository = userRepository;
    }
    public async Task AddChatAsync(ChatModel newChat, List<int> userIds)
    {
        if (userIds.Distinct().Count() != 2)
        {
            throw new ArgumentException("Ids must be distinct");
        }

        var thisUser = await _userRepository.FindUserByIdAsync(userIds[0]);
        var user2 = await _userRepository.FindUserByIdAsync(userIds[1]);

        if (thisUser == null || user2 == null)
        {
            throw new UserNotFoundException();
        }

        if (await _userChatRepository.CheckExistingChatAsync(userIds[0], userIds[1]))
        {
            throw new ExistedChatException();
        }

        if (!await _userRepository.CheckContactThisUserExistAsync(userIds[0], userIds[1]))
        {
            throw new ForbiddenOperationExceprion();
        }

        var chatsThisUser = await _userChatRepository.GetChatsByUserIdAsync(userIds[0]);

        if (chatsThisUser.Any(chat => chat.Name == newChat.Name))
        {
            throw new ChatNameExistedException();
        }

        var chatEntity = _mapper.Map<ChatEntity>(newChat);
        UserChatEntity userChat1 = new()
        {
            UserId = userIds[0]
        };
        UserChatEntity userChat2 = new()
        {
            UserId = userIds[1]
        };
        chatEntity.UserChats = new List<UserChatEntity>()
        {
            userChat1, userChat2
        };

        chatEntity.Name = $"{thisUser.Name} and {user2.Name}";
        userChat1.Chat = chatEntity;
        userChat2.Chat = chatEntity;

        await _chatRepository.CreateChatAsync(chatEntity);
    }

    public async Task DeleteChatAsync(int chatId, int ownerId)
    {
        var chatEntity = await _chatRepository.GetChatAsync(chatId);
        if (chatEntity == null)
        {
            throw new ChatNotFoundException();
        }

        var users = await _userChatRepository.GetUsersByChatIdAsync(chatId);

        if (users == null || users.All(u => u.Id != ownerId))
        {
            throw new UserNotFoundException();
        }
        await _chatRepository.DeleteChatAsync(chatId);
    }

    public async Task<List<SearchChatEntity>> SearchChatsByCriteriaAsync(string search, int currentUserId)
    {
        return await _chatRepository.SearchChatByCriteriaAsync(currentUserId, search);
    }

    public async Task<List<ChatBasicModel>> GetChatsAsync(int userId)
    {
        var userChats = await _chatRepository.GetAllChatsAsync(userId);
        var resultList = userChats
            .Select(uc => new ChatBasicModel
            {
                ChatId = uc.ChatId, ChatName = uc.Chat.Name ?? "Deleted or unknown chat"
            })
            .ToList();
        return resultList;
    }
}