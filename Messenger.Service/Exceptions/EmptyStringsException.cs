namespace Messenger.Service.Exceptions;

public class EmptyStringsException : Exception
{
    public EmptyStringsException() { }
    public EmptyStringsException(string message) : base(message) { }
    public EmptyStringsException(string message, Exception inner) : base(message, inner) { }
}