namespace Messenger.Service.Exceptions;

public class EmptyStringChangesException : Exception
{
    public EmptyStringChangesException(){}
    public EmptyStringChangesException(string message) : base(message){}
    public EmptyStringChangesException(string message, Exception inner) : base(message, inner){}
}