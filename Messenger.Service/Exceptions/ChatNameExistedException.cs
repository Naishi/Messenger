namespace Messenger.Service.Exceptions;

public class ChatNameExistedException : Exception
{
    public ChatNameExistedException() { }
    public ChatNameExistedException(string message) : base(message) { }
    public ChatNameExistedException(string message, Exception inner) : base(message, inner) { }
}