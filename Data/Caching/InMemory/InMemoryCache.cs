using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Data.Caching.InMemory;
public class InMemoryCache<T>(IConfiguration configuration, IMemoryCache cache) where T : class
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IMemoryCache _cache = cache;

    public T? Get(string key)
    {
        return _cache.Get<T>(key);
    }

    public void Set(string key, T value)
    {
        // TODO: handle null and invalid
        _cache.Set(key, value, TimeSpan.FromMinutes(int.Parse(_configuration["CacheTTLInMinutes"])));
    }

    public void Delete(string key)
    {
        _cache.Remove(key);
    }
}
