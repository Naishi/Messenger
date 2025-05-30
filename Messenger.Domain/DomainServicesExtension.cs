using Messenger.Domain.Data;
using Messenger.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Domain;

public static class DomainServicesExtension
{
    public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ChatRepository>();

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}