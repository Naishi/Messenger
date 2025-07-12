namespace Messenger.Service.Exceptions
{
    public class UndefinedUserRoleException : Exception
    {
        public UndefinedUserRoleException() { }

        public UndefinedUserRoleException(string? message) : base(message) { }

        public UndefinedUserRoleException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
