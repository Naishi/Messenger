namespace Messenger.Service.Exceptions;

public class LenghtOfMessageException : Exception
{
    public LenghtOfMessageException() { }

    public LenghtOfMessageException(string? message) : base(message) { }

    public LenghtOfMessageException(string? message, Exception? innerException) : base(message, innerException) { }
}