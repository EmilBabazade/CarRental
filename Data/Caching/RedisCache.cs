using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Data.Caching;
public class RedisCache<T>(IConfiguration config, IDistributedCache cache) : ICache<T> where T : class
{
    private readonly IConfiguration _config = config;
    private readonly IDistributedCache _cache = cache;

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

    public async Task<T?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        //var val = await _cache.GetStringAsync(_config["Redis:Prefix"] + key, cancellationToken);
        var val = await _cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(val)) return null;

        return JsonSerializer.Deserialize<T>(val);
    }

    public async Task SetAsync(string key, T value, CancellationToken cancellationToken = default)
    {
        var expiresInMinutes = _config["CacheTTLInMinutes"] == null ? 30 : int.Parse(_config["CacheTTLInMinutes"]);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiresInMinutes)
        };

        var json = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, json, options, cancellationToken);
    }
}
