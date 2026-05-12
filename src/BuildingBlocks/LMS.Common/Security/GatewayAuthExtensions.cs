using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Common.Security;

public static class GatewayAuthExtensions
{
    public static IServiceCollection AddGatewayAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Gateway")
            .AddScheme<AuthenticationSchemeOptions, GatewayAuthenticationHandler>("Gateway", _ => { });

        services.AddAuthorization();
        return services;
    }
}
