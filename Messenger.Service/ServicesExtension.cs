using Messenger.Domain.Entities;
using Messenger.Domain.Interfaces;
using Messenger.Domain.Repositories;
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
        //services.AddTransient<ChatService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<UserAuthEntity>, PasswordHasher<UserAuthEntity>>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailValidator, EmailValidator>();
        services.AddScoped<IProfanityFilter, ProfanityFilter.ProfanityFilter>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMessageService, MessageService>();
    }
}