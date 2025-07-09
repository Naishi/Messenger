using Messenger.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Service;

public static class ServicesExtension
{
    public static void AddMessengerServices(this IServiceCollection services)
    {
        services.AddTransient<ChatService>();
        services.AddTransient<AuthService>();
    }
}