using Messenger.Domain.Data;
using Messenger.Domain.Interfaces;
using Messenger.Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Domain;

public static class DomainServicesExtension
{
    public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAuthRepository, UserAuthRepository>();
        services.AddScoped<IUserChatRepository, UserChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ExecutionStrategyRetrying));
        });
    }
}