using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;
using Shorty.API.Features.Counter;

namespace Shorty.API.Features.Urls;

public interface IUrlRepository
{
    Task<string>  SaveAsync(string url , CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string token, CancellationToken cancellationToken = default);
}

internal sealed class UrlRepository(IDistributedCache cache, ICounterService counter, NpgsqlConnection db) : IUrlRepository
{
    public async Task<string> SaveAsync(string url, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            INSERT INTO shorty_urls (token, url, created_at)
            VALUES (@value, @url, @createdAt);
            """;

        var createdAt = DateTime.UtcNow;
        var count = await counter.IncrementAsync();
        string value = ShortUrlToken.NewToken(count);
        
        await db.ExecuteAsync(sql, new { value, url, createdAt });
        await SaveToCacheAsync(value, url, cancellationToken);
        
        return value;
    }

    public async Task<string?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var url = await cache.GetStringAsync(token, cancellationToken);
        if (!string.IsNullOrEmpty(url)) return url;

        const string sql = 
            """
            SELECT url FROM shorty_urls 
            WHERE token = @token
            LIMIT 1
            """;

        url = await db.QueryFirstOrDefaultAsync<string>(sql, new { token });
        await SaveToCacheAsync(token, url, cancellationToken);

        return url;
    }

    private async Task SaveToCacheAsync(string token, string? url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url)) return;

        var options = CreateDefaultOptions();
        await cache.SetStringAsync(token, url, options, cancellationToken);
    }

    private static DistributedCacheEntryOptions CreateDefaultOptions() => new DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromHours(1),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
    };
}
