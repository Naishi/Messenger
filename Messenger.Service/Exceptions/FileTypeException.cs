namespace Messenger.Service.Exceptions;

public class FileTypeException : Exception
{
    public FileTypeException() { }

    public FileTypeException(string? message) : base(message) { }

    public FileTypeException(string? messege, Exception? innerException) : base(messege, innerException) { }
}