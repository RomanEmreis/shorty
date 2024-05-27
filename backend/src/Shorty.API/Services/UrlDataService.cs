using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;

namespace Shorty.API.Services;

internal sealed class UrlDataService(IDistributedCache cache, NpgsqlConnection db) : IUrlDataService
{
    public async Task<string> SaveAsync(string url, CancellationToken cancellationToken = default)
    {
        var token  = new ShortUrlToken(url);
        var value  = token.GetValue();

        const string sql = """
            INSERT INTO shorty_urls (url, token)
            VALUES (@url, @value)
            """;

        _ = await db.ExecuteScalarAsync(sql, new { url, value });
        await SaveToCacheAsync(value, url, cancellationToken);

        return value;
    }

    public async Task<string> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var url = await cache.GetStringAsync(token, cancellationToken);
        if (string.IsNullOrEmpty(url))
        {
            const string sql = """
                SELECT url 
                FROM shorty_urls 
                WHERE token = @token
                """;

            url = await db.QueryFirstAsync<string>(sql, new { token });
            await SaveToCacheAsync(token, url, cancellationToken);
        }

        return url;
    }

    private async Task SaveToCacheAsync(string token, string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url)) return;

        var options = CreateDefaultOptions();
        await cache.SetStringAsync(token, url, cancellationToken);
    }

    private static DistributedCacheEntryOptions CreateDefaultOptions() => new DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromDays(3),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };
}
