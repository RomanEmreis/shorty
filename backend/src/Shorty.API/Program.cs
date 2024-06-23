using Shorty.API.Features.Urls;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("shorty-cache");
builder.AddNpgsqlDataSource("shorty-db");

builder.Services.AddRequestTimeouts(
    static options => options.DefaultPolicy = new() { Timeout = TimeSpan.FromMilliseconds(300) });

builder.Services.ConfigureHttpJsonOptions(
    static options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

builder.Services.AddProblemDetails();

builder.Services.AddScoped<IUrlRepository, UrlRepository>();

var app = builder.Build();

app.UseRequestTimeouts();

app.MapDefaultEndpoints();

app.MapPost("/create", async (IUrlRepository urlService, CreateShortUrl command, CancellationToken cancellationToken) => 
{
    var token = await urlService.SaveAsync(command.Url, cancellationToken);
    return Results.Ok(token);
});

app.MapGet("/{token}", async (IUrlRepository urlService, string token, CancellationToken cancellationToken) => 
{
    var url = await urlService.GetAsync(token, cancellationToken);
    return string.IsNullOrEmpty(url)
        ? Results.NotFound()
        : Results.Redirect(url);
});

app.MapGet("/health", async () => await Task.FromResult(Results.Ok("healthy")));

app.Run();


internal sealed record CreateShortUrl(string Url);

[JsonSerializable(typeof(CreateShortUrl))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}
