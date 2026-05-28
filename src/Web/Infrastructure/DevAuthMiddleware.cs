using System.Security.Claims;
using System.Security.Principal;

namespace SCMApp3.Web.Infrastructure;

/// <summary>
/// Development-only middleware that injects a fake Windows identity from configuration.
/// Activated only when ASPNETCORE_ENVIRONMENT=Development.
/// Configure test users in appsettings.Development.json under DevAuth.
/// </summary>
public class DevAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly DevAuthOptions _options;

    public DevAuthMiddleware(RequestDelegate next, DevAuthOptions options)
    {
        _next = next;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!string.IsNullOrWhiteSpace(_options.UserName))
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _options.UserName)
            };

            foreach (var role in _options.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new GenericIdentity(_options.UserName, "DevAuth");
            var principal = new ClaimsPrincipal(new ClaimsIdentity(identity, claims));
            context.User = principal;
        }

        await _next(context);
    }
}

public class DevAuthOptions
{
    public string UserName { get; set; } = "DEV\\developer";
    public List<string> Roles { get; set; } = ["ScmManager"];
}
