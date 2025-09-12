using Messenger.Domain.Entities;
using Messenger.Service.Interfaces;
using Messenger.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProfanityFilter.Interfaces;

namespace Messenger.Service;

public static class ServicesExtension
{
    public static void AddMessengerServices(this IServiceCollection services)
    {
        services.AddTransient<ChatService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IPasswordHasher<UserAuthEntity>, PasswordHasher<UserAuthEntity>>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEmailValidator, EmailValidator>();
        services.AddTransient<IProfanityFilter, ProfanityFilter.ProfanityFilter>();
    }
}