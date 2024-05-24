var builder = DistributedApplication.CreateBuilder(args);

// backend
var shortyDbName = "shorty-db";

var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", shortyDbName)
    .WithBindMount("../Shorty.API/Data", "/docker-entrypoint-initdb.d")
    .WithPgAdmin()
    .AddDatabase(shortyDbName);

var redis = builder.AddRedis("shorty-cache")
    .WithRedisCommander();

var api = builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(redis)
    .WithReference(postgres);

// frontend
builder.AddNpmApp("frontend", "../../../frontend")
    .WithReference(api)
    .WithHttpsEndpoint(env: "PORT");

builder.Build().Run();
