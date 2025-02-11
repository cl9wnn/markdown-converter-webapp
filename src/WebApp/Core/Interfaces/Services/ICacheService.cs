namespace Core.Interfaces.Services;

public interface ICacheService
{
    Task SetValueAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetValueAsync<T>(string key);
    Task RemoveValueAsync(string key);
}