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
    .WithRedisInsight();

// web api
var api = builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(postgres)
    .WithReference(redis)
    .WaitFor(postgres)
    .WaitFor(redis)
    .WithReplicas(3);

// reverse proxy
var proxy = builder.AddYarp("ingress")
    .WithHttpsEndpoint()
    .WithReference(api)
    .LoadFromConfiguration("ReverseProxy");

// frontend
builder.AddNpmApp("shorty-app", "../../../frontend/shorty-app")
    .WithReference(proxy)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
