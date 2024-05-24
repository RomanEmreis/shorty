var builder = DistributedApplication.CreateBuilder(args);

// backend
var redis = builder.AddRedis("shorty-cache")
    .WithRedisCommander();

var api = builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(redis);

// frontend
builder.AddNpmApp("frontend", "../../../frontend")
    .WithReference(api)
    .WithHttpsEndpoint(env: "PORT");

builder.Build().Run();
