namespace Data.Caching;

public interface ICache<T> where T : class
{
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
    Task SetAsync(string key, T value, CancellationToken cancellationToken = default);
}