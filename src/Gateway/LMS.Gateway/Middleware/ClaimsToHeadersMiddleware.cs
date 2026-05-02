using System;
using System.Security.Claims;

namespace LMS.Gateway.Middleware;

/// <summary>
/// Middleware that extracts user claims from the authenticated user and adds them as headers to the request.   
/// </summary>
/// <param name="next"></param>
public sealed class ClaimsToHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim != null)
            {
                context.Request.Headers["X-User-Id"] = userIdClaim.Value;
            }

            if (emailClaim != null)
            {
                context.Request.Headers["X-User-Email"] = emailClaim.Value;
            }

            if (roleClaim != null)
            {
                context.Request.Headers["X-User-Role"] = roleClaim.Value;
            }
        }

        await next(context);
    }
}
