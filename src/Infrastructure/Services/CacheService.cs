using Microsoft.Extensions.Caching.Memory;
using SCMApp3.Application.Common.Interfaces;

namespace SCMApp3.Infrastructure.Services;

public class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(15);
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache) => _cache = cache;

    public T? Get<T>(string key) => _cache.TryGetValue(key, out T? value) ? value : default;

    public void Set<T>(string key, T value, TimeSpan? ttl = null)
        => _cache.Set(key, value, ttl ?? DefaultTtl);

    public void Remove(string key) => _cache.Remove(key);
}
