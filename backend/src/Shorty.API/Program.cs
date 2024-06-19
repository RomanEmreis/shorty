using Shorty.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("shorty-cache");
builder.AddNpgsqlDataSource("shorty-db");

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));
builder.Services.AddCors();

builder.Services.AddScoped<IUrlRepository, UrlRepository>();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapDefaultEndpoints();

app.MapPost("/", async (IUrlRepository urlService, CreateShortUrl command, CancellationToken cancellationToken) => 
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

app.Run();


internal sealed record CreateShortUrl(string Url);

[JsonSerializable(typeof(CreateShortUrl))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}
