namespace Messenger.Service.Exceptions;

public class ImproperUserException : Exception
{
    public ImproperUserException() { }
    public ImproperUserException(string message) : base(message) { }
    public ImproperUserException(string message, Exception inner) : base(message, inner) { }
}