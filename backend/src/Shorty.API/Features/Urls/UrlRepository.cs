using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;

namespace Shorty.API.Features.Urls;

public interface IUrlRepository
{
    Task<string>  SaveAsync(string url , CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string token, CancellationToken cancellationToken = default);
}

internal sealed class UrlRepository(IDistributedCache cache, NpgsqlConnection db) : IUrlRepository
{
    public async Task<string> SaveAsync(string url, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            INSERT INTO shorty_urls (token, url, created_at)
            VALUES (@value, @url, @createdAt)
            ON CONFLICT (token) DO NOTHING;
            """;

        var token        = new ShortUrlToken();
        var createdAt    = DateTime.UtcNow;
        var value        = string.Empty;
        var count        = 0;

        while (count == 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            value        = token.GetValue();
            count        = await db.ExecuteAsync(sql, new { value, url, createdAt });
        }

        await SaveToCacheAsync(value, url, cancellationToken);

        return value;
    }

    public async Task<string?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var url              = await cache.GetStringAsync(token, cancellationToken);
        if (string.IsNullOrEmpty(url))
        {
            const string sql = 
                """
                SELECT url 
                FROM shorty_urls 
                WHERE token = @token
                LIMIT 1
                """;

            url              = await db.QueryFirstOrDefaultAsync<string>(sql, new { token });

            if (!string.IsNullOrEmpty(url))
            {
                await SaveToCacheAsync(token, url, cancellationToken);
            }
        }

        return url;
    }

    private async Task SaveToCacheAsync(string token, string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url)) return;

        var options = CreateDefaultOptions();
        await cache.SetStringAsync(token, url, options, cancellationToken);
    }

    private static DistributedCacheEntryOptions CreateDefaultOptions() => new DistributedCacheEntryOptions
    {
        SlidingExpiration               = TimeSpan.FromHours(5),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };
}
