namespace Messenger.Service.Exceptions;

public class UserLoginRequestException : Exception
{
    public UserLoginRequestException() { }

    public UserLoginRequestException(string? message) : base(message) { }

    public UserLoginRequestException(string? message, Exception? innerException) : base(message, innerException) { }
}