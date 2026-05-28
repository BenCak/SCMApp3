using SCMApp3.Application.Common.Interfaces;

namespace SCMApp3.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    public Task<string?> GetUserNameAsync(string userId)
    {
        // In this local dev / Windows Negotiate auth system, the userId is the username.
        return Task.FromResult<string?>(userId);
    }

    public bool IsInRole(string userId, string role)
    {
        // Default role authorization - in a POC, let all authenticated users match roles.
        return true;
    }

    public Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        // Default policy authorization - let all policies pass for local POC.
        return Task.FromResult(true);
    }
}
