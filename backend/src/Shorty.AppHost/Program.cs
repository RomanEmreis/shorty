var builder = DistributedApplication.CreateBuilder(args);

// backend
var shortyDbName = "shorty-db";

// database
var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", shortyDbName)
    .WithBindMount("../Shorty.API/Data", "/docker-entrypoint-initdb.d")
    .WithPgAdmin()
    .AddDatabase(shortyDbName);

// cache
var redis = builder.AddRedis("shorty-cache")
    .WithRedisCommander();

// web api
var api = builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(redis)
    .WithReference(postgres);

// reverse proxy
var proxy = builder.AddYarp("ingress")
    .WithHttpsEndpoint()
    .WithReference(api)
    .LoadFromConfiguration("ReverseProxy");

// frontend
builder.AddNpmApp("frontend", "../../../frontend")
    .WithReference(proxy)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
