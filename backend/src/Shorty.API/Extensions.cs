using MongoDB.Driver;
using Shorty.API.Features.Urls;

namespace Shorty.API;

internal static class Extensions
{
    private const string CollectionName = "shorty-urls";

    internal static IServiceCollection AddShortyUrlsCollection(this IServiceCollection services)
    {
        return services.AddScoped(provider => 
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<ShortUrl>(CollectionName);
        });
    }

    internal static IApplicationBuilder UseShortyUrlsCollection(this IApplicationBuilder applicationBuilder)
    {
        var database              = applicationBuilder.ApplicationServices.GetRequiredService<IMongoDatabase>();

        var clusteredIndexOptions = new ClusteredIndexOptions<ShortUrl>
        {
            Key                   = Builders<ShortUrl>.IndexKeys.Ascending(r => r.Token),
            Unique                = true
        };

        var options               = new CreateCollectionOptions<ShortUrl>
        {
            ClusteredIndex        = clusteredIndexOptions
        };

        database.CreateCollectionAsync(CollectionName, options);

        var collection            = database.GetCollection<ShortUrl>(CollectionName);
        var indexModel            = new CreateIndexModel<ShortUrl>(Builders<ShortUrl>.IndexKeys.Ascending(m => m.Url));

        collection.Indexes.CreateOne(indexModel);

        return applicationBuilder;
    }
}
