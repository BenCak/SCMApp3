using SCMApp3.Domain.Enums;

namespace SCMApp3.Domain.Services;

public static class StatusTransitionValidator
{
    private static readonly Dictionary<Status, HashSet<Status>> AllowedTransitions = new()
    {
        [Status.Pending]      = [Status.InReview],
        [Status.InReview]     = [Status.NeedMoreInfo, Status.Rejected, Status.Released],
        [Status.NeedMoreInfo] = [Status.Pending],
        [Status.Rejected]     = [],
        [Status.Released]     = [Status.Pending],
    };

    public static bool Validate(Status from, Status to)
        => AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static void ValidateOrThrow(Status from, Status to)
    {
        if (!Validate(from, to))
            throw new InvalidOperationException($"Status transition from {from} to {to} is not allowed.");
    }
}
