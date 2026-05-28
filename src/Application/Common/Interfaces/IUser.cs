namespace SCMApp3.Application.Common.Interfaces;

public interface IUser
{
    string? UserName { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsInRole(string role);
}
