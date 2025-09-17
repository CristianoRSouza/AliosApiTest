using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Ailos.ContaCorrente.API.Services;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            if (_cache.TryGetValue(key, out var cached))
            {
                _logger.LogInformation("Cache HIT: {Key}", key);
                return (T)cached;
            }
            
            _logger.LogInformation("Cache MISS: {Key}", key);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar no cache: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            }

            _cache.Set(key, value, options);
            _logger.LogInformation("Cache SET: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar no cache: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            _cache.Remove(key);
            _logger.LogInformation("Cache REMOVE: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover do cache: {Key}", key);
        }
    }
}
