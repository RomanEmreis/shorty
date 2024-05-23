using Microsoft.Extensions.Caching.Distributed;
using Shorty.API;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("shorty-cache");
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/", async (IDistributedCache cache, IHttpContextAccessor httpContextAccessor, CreateShortUrl command, CancellationToken cancellationToken) => 
{
    var url = command.Url;

    if (!url.StartsWith("http"))
    {
        url = $"http://{url}";
    }

    var token = new ShortUrlToken(url);
    var value = token.GetValue();

    var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
    var host = httpContextAccessor.HttpContext!.Request.Host;

    var options = new DistributedCacheEntryOptions 
    { 
        SlidingExpiration = TimeSpan.FromDays(3),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };

    await cache.SetStringAsync(value, url, cancellationToken);

    return Results.Ok($"{scheme}://{host}/{value}");
});

app.MapGet("/{token}", async (IDistributedCache cache, string token, CancellationToken cancellationToken) => 
{
    var url = await cache.GetStringAsync(token, cancellationToken);

    return string.IsNullOrEmpty(url) ? Results.NotFound() : Results.Redirect(url);
});

app.Run();


internal sealed record CreateShortUrl(string Url);

[JsonSerializable(typeof(CreateShortUrl))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}
