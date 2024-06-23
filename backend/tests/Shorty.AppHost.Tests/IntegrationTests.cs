using FluentAssertions;
using System.Net;

namespace Shorty.AppHost.Tests;

public class IntegrationTests
{
    //[Fact]
    //public async Task GetFrontendAppRoot_ReturnsOkStatusCode()
    //{
    //    var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
    //    await using var app = await appHost.BuildAsync();
    //    await app.StartAsync();

    //    var httpClient = app.CreateHttpClient("frontend");
    //    var response = await httpClient.GetAsync("/");

    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //}

    [Fact]
    public async Task FrontendResource_ShouldBeAvailable()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var frontendResource = appHost.Resources.Single(resource => resource.Name == "frontend");

        frontendResource.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAPIHealth_ReturnsOKStatusCode()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var httpClient = app.CreateHttpClient("shorty-api");
        var response = await httpClient.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task IngressResource_ShouldBeAvailable()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var ingressResource = appHost.Resources.Single(resource => resource.Name == "ingress");

        ingressResource.Should().NotBeNull();
    }

    [Fact]
    public async Task PostgresResource_ShouldBeAvailable()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var postgresResource = appHost.Resources.Single(resource => resource.Name == "postgres");

        postgresResource.Should().NotBeNull();
    }

    [Fact]
    public async Task RedisResource_ShouldBeAvailable()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var redisResource = appHost.Resources.Single(resource => resource.Name == "shorty-cache");

        redisResource.Should().NotBeNull();
    }
}