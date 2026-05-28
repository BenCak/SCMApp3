namespace SCMApp3.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);
    bool IsInRole(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
}
