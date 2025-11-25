namespace Messenger.Service.Exceptions;

public class ExistedChatException : Exception
{
    public ExistedChatException() { }
    public ExistedChatException(string message) : base(message) { }
    public ExistedChatException(string message, Exception inner) : base(message, inner) { }
}