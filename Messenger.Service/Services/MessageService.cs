using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;

namespace Messenger.Service.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserChatRepository _userChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public MessageService(IMessageRepository messageRepository,
        IMapper mapper,
        IUserChatRepository userChatRepository,
        IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userChatRepository = userChatRepository;
        _userRepository = userRepository;
    }
    
    public async Task CreateMessageAsync(MessageModel message, int userId)
    {
        if (message.Text.Length is 0 or >= 256)
        {
            throw new LenghtOfMessageException();
        }

        var userChatsEntities = await _userChatRepository.GetAsync(
            new()
            {
                UserIds = [userId],
                ChatIds = [message.ChatId]
            });

        if (userChatsEntities.Count == 0)
        {
            throw new ChatNotFoundException("Chat not found or you are not existed");
        }
        
        var messageEntity = _mapper.Map<MessageEntity>(message);
        messageEntity.UserId = userId;
        await _messageRepository.CreateAsync(messageEntity);
    }

    public async Task RemoveMessageAsync(int messageId, int userId)
    {
        var messageEntities = await _messageRepository.GetAsync(new()
        {
            MessageId = messageId
        });

        var userEntities = await _userRepository.GetAsync(
            new()
            {
                ChatId = messageEntities[0].ChatId
            });
        
        if (messageEntities.Count == 0 || userEntities.Count == 0 || userEntities.Any(u =>  u.Id == userId))
        {
            throw new MessageNotFoundException();
        }

        var message = messageEntities[0];
        await _messageRepository.DeleteAsync(message);
    }

    public async Task ModifyMessageAsync(int messageId, string text, int userId)
    {
        if (messageId <= 0 || string.IsNullOrWhiteSpace(text))
        {
            throw new MessageModifyException();
        }

        var messageEntities = await _messageRepository.GetAsync(
            new()
            {
                MessageId = messageId
            });

        if (messageEntities.Count == 0 || messageEntities.Any(m => m.UserId != userId))
        {
            throw new MessageNotFoundException();
        }

        var message = messageEntities[0];
        message.Text = text;
        
        await _messageRepository.UpdateAsync(message);
    }

    public async Task<List<MessageModel>> GetAllMessagesAsync(int userId, int chatId)
    {
        var messagesList = await _messageRepository.GetAsync(new()
        {
            ChatId = chatId,
            UserId = userId
        });

        return _mapper.Map<List<MessageModel>>(messagesList);
    }

    public async Task<MessageModel> GetMessageAsync(int messageId, int userId)
    {
        var message = await _messageRepository.GetAsync(new()
        {
            MessageId = messageId
        });

        if (message.Count == 0 || message[0].UserId != userId)
        {
            throw new MessageNotFoundException();
        }

        return _mapper.Map<MessageModel>(message);
    }
}