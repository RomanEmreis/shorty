using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace Shorty.API.Features.Urls;

public interface IUrlRepository
{
    Task<string> SaveAsync(string url, CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string token, CancellationToken cancellationToken = default);
}

internal sealed class UrlRepository(IDistributedCache cache, IMongoCollection<ShortUrl> collection) : IUrlRepository
{
    public async Task<string> SaveAsync(string url, CancellationToken cancellationToken = default)
    {
        var value      = await GetTokenByUrlAsync(url, cancellationToken);
        if (string.IsNullOrEmpty(value))
        {
            var token = new ShortUrlToken();
            do
            {
                value = token.GetValue();
            }
            while (await CheckIfTokenAlreadyInUse(value, cancellationToken));

            await collection.InsertOneAsync(
                document: new(value, url),
                cancellationToken: cancellationToken);
        }

        await SaveToCacheAsync(value, url, cancellationToken);

        return value;
    }

    private async Task<string?> GetTokenByUrlAsync(string url, CancellationToken cancellationToken)
    {
        var query      = Builders<ShortUrl>.Filter.Eq(shortUrl => shortUrl.Url, url);
        var options    = new FindOptions<ShortUrl, string?>
        {
            Projection = Builders<ShortUrl>.Projection.Include(nameof(ShortUrl.Token)),
            Limit      = 1
        };

        var cursor     = await collection.FindAsync(query, options, cancellationToken);
        
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var url = await cache.GetStringAsync(token, cancellationToken);
        if (string.IsNullOrEmpty(url))
        {
            url = await GetUrlFromDatabaseAsync(token, cancellationToken);

            if (!string.IsNullOrEmpty(url))
            {
                await SaveToCacheAsync(token, url, cancellationToken);
            }
        }

        return url;
    }

    private async Task<string?> GetUrlFromDatabaseAsync(string token, CancellationToken cancellationToken = default)
    {
        var query      = Builders<ShortUrl>.Filter.Eq(nameof(ShortUrl.Token), token);
        var options    = new FindOptions<ShortUrl, string?>
        {
            Projection = Builders<ShortUrl>.Projection.Include(nameof(ShortUrl.Url)),
            Limit      = 1
        };

        var cursor     = await collection.FindAsync(query, options, cancellationToken);
        
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<bool> CheckIfTokenAlreadyInUse(string token, CancellationToken cancellationToken = default)
    {
        var query   = Builders<ShortUrl>.Filter.Eq(nameof(ShortUrl.Token), token);
        var options = new CountOptions
        {
            Limit   = 1
        };

        var count   = await collection.CountDocumentsAsync(query, options, cancellationToken);

        return count > 0;
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
