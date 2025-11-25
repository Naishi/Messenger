namespace Messenger.Service.Exceptions;

public class ProfanityExistException : Exception
{
    public ProfanityExistException() { }
    public ProfanityExistException(string message) : base(message) { }
    public ProfanityExistException(string message, Exception inner) : base(message, inner) { }
}