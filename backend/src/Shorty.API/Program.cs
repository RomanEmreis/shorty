using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;
using Shorty.API;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("shorty-cache");
builder.AddNpgsqlDataSource("shorty-db");

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapDefaultEndpoints();

app.MapPost("/", async (IDistributedCache cache, NpgsqlConnection db, IHttpContextAccessor httpContextAccessor, CreateShortUrl command, CancellationToken cancellationToken) => 
{
    var url = command.Url;
    var token = new ShortUrlToken(url);
    var value = token.GetValue();

    var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
    var host = httpContextAccessor.HttpContext!.Request.Host;

    const string sql = """
        INSERT INTO shorty_urls (url, token)
        VALUES (@url, @value)
        """;

    _ = await db.ExecuteScalarAsync(sql, new { url, value });

    var options = new DistributedCacheEntryOptions 
    { 
        SlidingExpiration = TimeSpan.FromDays(3),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };

    await cache.SetStringAsync(value, url, cancellationToken);

    return Results.Ok($"{scheme}://{host}/{value}");
});

app.MapGet("/{token}", async (IDistributedCache cache, NpgsqlConnection db, string token, CancellationToken cancellationToken) => 
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

        if (!string.IsNullOrEmpty(url))
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
            };

            await cache.SetStringAsync(token, url, cancellationToken);
        }
        else 
        {
            return Results.NotFound();
        }
    }

    return Results.Redirect(url);
});

app.Run();


internal sealed record CreateShortUrl(string Url);

[JsonSerializable(typeof(CreateShortUrl))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}
