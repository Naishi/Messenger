namespace Messenger.Service.Exceptions;

public class BirthDateException : Exception
{
    public BirthDateException() { }

    public BirthDateException(string? message) : base(message) { }

    public BirthDateException(string? message, Exception? innerException) : base(message, innerException) { }
}