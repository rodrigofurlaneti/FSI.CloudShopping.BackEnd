namespace FSI.CloudShopping.Infrastructure.Services;

using Microsoft.Extensions.Caching.Memory;
using FSI.CloudShopping.Application.Interfaces;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        return await Task.FromResult(_memoryCache.TryGetValue(key, out T? value) ? value : null);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        var cacheOptions = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            cacheOptions.AbsoluteExpirationRelativeToNow = expiration;
        }
        else
        {
            cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
        }

        _memoryCache.Set(key, value, cacheOptions);
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);
        await Task.CompletedTask;
    }

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // IMemoryCache doesn't support pattern removal. This is a limitation.
        // In production, consider using Redis for better cache management.
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
    }
}
