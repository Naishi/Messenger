using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using DnsClient;
using Messenger.Service.Interfaces;

namespace Messenger.Service;

public class EmailValidator : IEmailValidator
{
    public bool IsEmailSyntaxValid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> IsEmailDomainValidAsync(string email)
    {
        if(string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return false;
        var domain = email.Substring(email.IndexOf('@') + 1);

        try
        {
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domain, QueryType.MX);
            return result.Answers.MxRecords().Any();
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsEmailValidAndExistAsync(string email)
    {
        if(!IsEmailSyntaxValid(email))
            return false;
        if(!await IsEmailDomainValidAsync(email))
            return false;
        
        return true;
    }
}