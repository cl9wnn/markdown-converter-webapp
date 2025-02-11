using System.Text.Json;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class RedisCacheService(IDistributedCache cache): ICacheService
{
    public async Task SetValueAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };

        var jsonData = JsonSerializer.Serialize(value,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        await cache.SetStringAsync(key, jsonData, options);
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var jsonData = await cache.GetStringAsync(key);
        
        return jsonData is not null 
            ? JsonSerializer.Deserialize<T>(jsonData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) 
            : default;
    }

    public async Task RemoveValueAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}