using Core.Security.OtpAuthenticator;
using Core.Security.OtpAuthenticator.OtpNet;
using MGH.Security.EmailAuthenticator;
using MGH.Security.JWT;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        return services;
    }
}
