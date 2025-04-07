using ApiGateway.ReverseProxy.Options;
using Microsoft.Extensions.Options;

namespace ApiGateway.ReverseProxy.Middleware
{
    public class ScopedAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PublicEndpointOptions _publicEndpoints;

        public ScopedAuthenticationMiddleware(RequestDelegate next, IOptions<PublicEndpointOptions> publicEndpoints)
        {
            _next = next;
            _publicEndpoints = publicEndpoints.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? path = context.Request.Path.Value?.ToLowerInvariant();

            if (string.IsNullOrEmpty(path))
            {
                await _next(context);
                return;
            }

            // Tìm service tương ứng
            string? matchedService = _publicEndpoints.Keys.FirstOrDefault(service => path.StartsWith($"/api/v1/{service}"));

            if (matchedService is not null)
            {
                bool isPublic = _publicEndpoints[matchedService].Any(p => p.Equals(path, StringComparison.OrdinalIgnoreCase));

                if (!isPublic && (!context.User.Identity?.IsAuthenticated ?? true))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}