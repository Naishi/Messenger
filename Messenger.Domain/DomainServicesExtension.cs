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
        services.AddTransient<ChatRepository>();
        services.AddTransient<UserRepository>();
        services.AddTransient<IUserAuthRepository, UserAuthRepository>();

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ExecutionStrategyRetrying));
        });
    }
}