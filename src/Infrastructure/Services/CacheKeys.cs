namespace SCMApp3.Infrastructure.Services;

public static class CacheKeys
{
    public const string AllCustomers = "customers:all";
    public const string AllProducts = "products:all";

    public static string SystemsByCustomer(int customerId) => $"systems:customer:{customerId}";
    public static string SystemVersions(int systemId) => $"system-versions:{systemId}";
    public static string Segments(int systemVersionId) => $"segments:{systemVersionId}";
    public static string Cscis(int segmentId) => $"cscis:{segmentId}";
}
