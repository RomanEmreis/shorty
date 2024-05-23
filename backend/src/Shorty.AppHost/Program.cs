var builder = DistributedApplication.CreateBuilder(args);

// backend
var redis = builder.AddRedis("shorty-cache")
    .WithRedisCommander();

builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(redis);

builder.Build().Run();
