namespace ApiGateway.ReverseProxy.Middleware
{
    public class EndpointAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public EndpointAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? path = context.Request.Path.Value?.ToLowerInvariant();

            if (path is "/user/api/v1/users/login" or "/user/api/v1/users/register")
            {
                await _next(context);
                return;
            }

            if (path != null && (path.StartsWith("/swagger") || path.Contains("swagger.json")))
            {
                await _next(context);
                return;
            }

            if (path != null && path.StartsWith("/user"))
                if (!context.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

            await _next(context);
        }
    }
}