namespace Messenger.Service.Exceptions;

public class ForbiddenOperationExceprion : Exception
{
    public ForbiddenOperationExceprion() { }
    public ForbiddenOperationExceprion(string? msg) : base(msg) { }
    public ForbiddenOperationExceprion(string? msg, Exception? inner) : base(msg, inner) { }
}