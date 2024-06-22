using MongoDB.Bson.Serialization.Attributes;

namespace Shorty.API.Features.Urls;

internal sealed class ShortUrl(string token, string url)
{
    [BsonId]
    public string Token { get; set; } = token;

    public string Url { get; set; } = url;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
