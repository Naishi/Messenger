using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;

namespace Messenger.Service.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IContactRepository _contactRepository;

    public ChatService(
        IChatRepository chatRepository,
        IMapper mapper,
        IUserRepository userRepository,
        IContactRepository contactRepository)
    {
        _chatRepository = chatRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _contactRepository = contactRepository;
    }

    public async Task AddChatAsync(ChatModel newChat, List<int> userIds)
    {
        if (userIds.Distinct().Count() != 2)
        {
            throw new ArgumentException("Ids must be distinct");
        }

        UserFilter userFilter = new()
        {
            UserIds = [userIds[0], userIds[1]]
        };
        var users = await _userRepository.GetAsync(userFilter);
        var thisUser = users[0];
        var user2 = users[1];

        if (thisUser == null || user2 == null)
        {
            throw new UserNotFoundException();
        }

        ChatFilter chatFilter = new()
        {
            UserIds = [userIds[0], userIds[1]]
        };
        var chat = await _chatRepository.GetAsync(chatFilter);

        if (chat.Count != 0)
        {
            throw new ExistedChatException();
        }

        ContactFilter contactFilter = new()
        {
            OwnerUserId = thisUser.Id, ContactUserId = user2.Id
        };
        var contact = await _contactRepository.GetAsync(contactFilter);

        if (contact.Count == 0)
        {
            throw new ForbiddenOperationExceprion();
        }

        var newChatEntity = _mapper.Map<ChatEntity>(newChat);
        UserChatEntity userChat1 = new()
        {
            UserId = userIds[0]
        };
        UserChatEntity userChat2 = new()
        {
            UserId = userIds[1]
        };
        newChatEntity.UserChats = new List<UserChatEntity>()
        {
            userChat1, userChat2
        }!;

        newChatEntity.Name = $"{thisUser.Name} and {user2.Name}";
        userChat1.Chat = newChatEntity;
        userChat2.Chat = newChatEntity;

        await _chatRepository.CreateAsync(newChatEntity);
    }

    public async Task DeleteChatAsync(int chatId, int ownerId)
    {
        var filter = new ChatFilter();
        var chatEntities = await _chatRepository.GetAsync(filter);

        if (chatEntities.Count == 0)
        {
            throw new ChatNotFoundException();
        }

        if (chatEntities.Count > 1)
        {
            throw new ChatNotFoundException("Multiple chats???");
        }

        var userFilter = new UserFilter()
        {
            ChatId = chatId
        };
        
        var users = await _userRepository.GetAsync(userFilter);

        if (users.Count == 0 || users.All(u => u.Id != ownerId))
        {
            throw new UserNotFoundException();
        }

        await _chatRepository.DeleteAsync(chatEntities[0]);
    }

    public async Task<List<ChatModel>> SearchChatsByCriteriaAsync(string search, int currentUserId)
    {
        var chats = await _chatRepository.GetAsync(
            new()
            {
                UserIds = [currentUserId], Search = search
            });
        
        return _mapper.Map<List<ChatModel>>(chats);
    }

    public async Task<List<ChatModel>> GetChatsAsync(int userId)
    {
        var chats = await _chatRepository.GetAsync(new()
        {
            UserIds = [userId]
        });

        return _mapper.Map<List<ChatModel>>(chats);
    }
}