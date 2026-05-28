namespace SCMApp3.Web.Infrastructure;

public class UserActivityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserActivityMiddleware> _logger;

    public UserActivityMiddleware(RequestDelegate next, ILogger<UserActivityMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        var user = context.User?.Identity?.Name ?? "anonymous";
        _logger.LogInformation("ACTIVITY | {Method} {Path} | {User}",
            context.Request.Method, context.Request.Path, user);

        await _next(context);
    }
}
