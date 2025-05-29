using Domain.DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainService(this IServiceCollection services)
    {
        services.AddTransient<UserManager>();

        return services;
    }
}
