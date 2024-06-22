using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// backend
var shortyDbName = "shorty-db";

// database
var mongo = builder.AddMongoDB("mongo")
    .AddDatabase(shortyDbName);

// cache
var redis = builder.AddRedis("shorty-cache");

// web api
var api = builder.AddProject<Projects.Shorty_API>("shorty-api")
    .WithReference(redis)
    .WithReference(mongo);

var isDev = builder.Environment.IsDevelopment();
// Currently YARP is only available on development, so excluding it from the deployement
if (isDev)
{
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
}
else 
{
    // frontend
    builder.AddNpmApp("frontend", "../../../frontend")
        .WithReference(api)
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();
}

builder.Build().Run();
