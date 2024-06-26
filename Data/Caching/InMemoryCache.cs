using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Caching;
public class InMemoryCache<T>(IConfiguration configuration, IMemoryCache cache) : ICache<T> where T : class
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IMemoryCache _cache = cache;

    public async Task<T?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.Get<T>(key);
    }

    public async Task SetAsync(string key, T value, CancellationToken cancellationToken = default)
    {
        // TODO: put in extension method
        var expiresInMinutes = _configuration["CacheTTLInMinutes"] == null ? 30 : int.Parse(_configuration["CacheTTLInMinutes"]);
        _cache.Set(key, value, TimeSpan.FromMinutes(expiresInMinutes));
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        _cache.Remove(key);
    }
}
