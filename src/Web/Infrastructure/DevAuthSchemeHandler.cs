using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace SCMApp3.Web.Infrastructure;

/// <summary>
/// Trivial auth handler for development. The DevAuthMiddleware already sets HttpContext.User,
/// so this handler just succeeds for any request that reaches it.
/// </summary>
public class DevAuthSchemeHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DevAuthSchemeHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.User.Identity?.IsAuthenticated == true)
            return Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(Context.User, "DevAuth")));

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
