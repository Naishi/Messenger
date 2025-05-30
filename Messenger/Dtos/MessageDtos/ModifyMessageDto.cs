namespace Messenger.Dtos.MessageDtos;

public class ModifyMessageDto
{
    public int MessageId { get; set; }

    public int UserId { get; set; }//проверка тот ли это пользователь который написал сообщение

    public required string Text { get; set; }
}