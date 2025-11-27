using System.ComponentModel;

using AutoMapper;

using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Models;


namespace Messenger.Service.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    private readonly IUserChatRepository _userChatRepository;


    public MessageService(IMessageRepository messageRepository, IMapper mapper, IUserChatRepository userChatRepository)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userChatRepository = userChatRepository;
    }

    //todo:проверки пользователя и чата (есть ли такой чат, не забанен ли пользователь, может ли вообще пользователь
    //писать в этом чате)
    public async Task CreateMessageAsync(MessageModel message, int  userId)
    {
        if (message.Text.Length is 0 or >= 256)
        {
            throw new LenghtOfMessageException();
        }
        var messageEntity = _mapper.Map<MessageEntity>(message);
        await _messageRepository.CreateMessageAsync(messageEntity, userId);
    }

    public async Task RemoveMessageAsync(int messageId, int userId)
    {
        if (messageId is > 0)
        {
            await _messageRepository.RemoveMessageAsync(messageId, userId);
        }
    }

    public async Task ModifyMessageAsync(int messageId, string text, int userId)
    {
        if (messageId <= 0 || string.IsNullOrWhiteSpace(text))
        {
            throw new MessageModifyException();
        }
        
        await _messageRepository.ModifyMessageAsync(messageId, text, userId);
    }

    public async Task<List<MessageModel>> GetAllMessagesAsync(int userId, int chatId)
    {
         var messagesList = await _messageRepository.GetAllMessageAsync(chatId, userId);
         return _mapper.Map<List<MessageModel>>(messagesList);
    }

    public async Task<MessageModel> GetMessageAsync(int messageId, int userId)
    {
        var message = await _messageRepository.GetMessageByIdAsync(messageId, userId);
        return _mapper.Map<MessageModel>(message);
    }

}