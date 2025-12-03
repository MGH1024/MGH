using MGH.Core.Infrastructure.Securities.Security.JWT;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.Securities.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        return services;
    }
}
