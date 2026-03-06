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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<UserAuthEntity>, PasswordHasher<UserAuthEntity>>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailValidator, EmailValidator>();
        services.AddScoped<IProfanityFilter, ProfanityFilter.ProfanityFilter>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMessageService, MessageService>();
    }
}