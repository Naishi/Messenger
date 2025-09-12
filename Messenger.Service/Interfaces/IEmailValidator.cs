namespace Messenger.Service.Interfaces;

public interface IEmailValidator
{
    bool IsEmailSyntaxValid(string email);
    Task<bool> IsEmailDomainValidAsync(string email);
    Task<bool> IsEmailValidAndExistAsync(string email);
}