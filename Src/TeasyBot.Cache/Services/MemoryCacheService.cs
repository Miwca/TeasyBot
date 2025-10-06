using Microsoft.Extensions.Caching.Memory;
using TeasyBot.Cache.Services.Abstractions;

namespace TeasyBot.Cache.Services;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key)
    {
        var success = _memoryCache.TryGetValue<T>(key, out var result);
        return success ? result : default;
    }

    public void Set<T>(string key, T @object)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));

        _memoryCache.Set(key, @object, cacheEntryOptions);
    }
}