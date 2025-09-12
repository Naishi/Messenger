namespace Messenger.Service.Exceptions;

public class MessageModifyException : Exception
{
    public MessageModifyException() { }

    public MessageModifyException(string? message) : base(message) { }

    public MessageModifyException(string? message, Exception? innerException) : base(message, innerException) { }
}