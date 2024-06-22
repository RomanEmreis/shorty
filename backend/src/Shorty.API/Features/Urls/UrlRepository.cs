using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;
using System;

namespace Shorty.API.Features.Urls;

public interface IUrlRepository
{
    Task<string> SaveAsync(string url, CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string token, CancellationToken cancellationToken = default);
}

internal sealed class UrlRepository(IDistributedCache cache, NpgsqlConnection db) : IUrlRepository
{
    public async Task<string> SaveAsync(string url, CancellationToken cancellationToken = default)
    {
        var value = await GetTokenByUrlAsync(url, cancellationToken);
        if (string.IsNullOrEmpty(value))
        {
            var token = new ShortUrlToken();
            do
            {
                value = token.GetValue();
            }
            while (await CheckIfTokenIsInUseAsync(value, cancellationToken));
            
            const string sql = """
                INSERT INTO shorty_urls (url, token)
                VALUES (@url, @value)
                """;

            _ = await db.ExecuteScalarAsync(sql, new { url, value });
        }

        await SaveToCacheAsync(value, url, cancellationToken);

        return value;
    }

    private async Task<string?> GetTokenByUrlAsync(string url, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT token 
            FROM shorty_urls 
            WHERE url = @url
            LIMIT 1
            """;

        var token = await db.QueryFirstOrDefaultAsync<string?>(sql, new { url });
        return token;
    }

    private async Task<bool> CheckIfTokenIsInUseAsync(string token, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT EXISTS
            (
                SELECT 1 FROM shorty_urls WHERE token = @token
            )
            """;

        return await db.QueryFirstOrDefaultAsync<bool>(sql, new { token });
    }

    public async Task<string?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var url = await cache.GetStringAsync(token, cancellationToken);
        if (string.IsNullOrEmpty(url))
        {
            const string sql = """
                SELECT url 
                FROM shorty_urls 
                WHERE token = @token
                LIMIT 1
                """;

            url = await db.QueryFirstOrDefaultAsync<string>(sql, new { token });

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
        SlidingExpiration = TimeSpan.FromHours(5),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };
}
