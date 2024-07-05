using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Shorty.AppHost.Tests;

public class IntegrationTests : ShortyAppHostFactory
{
    [Fact]
    public async Task PostUrl_And_GetUrl_ReturnsOKStatusCode()
    {
        const string expectedUrl = "https://www.google.com";
        string token;
        
        // create short url
        using (var httpClient = _app!.CreateHttpClient("shorty-api"))
        {
            var content = JsonContent.Create(new { url = expectedUrl });
            var postResponse = await httpClient.PostAsync("/create", content);

            token = await postResponse.Content.ReadAsStringAsync();
            
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            token.Should().NotBeNullOrWhiteSpace();
        }

        // resolve short url
        using (var httpClient = _app!.CreateHttpClient("shorty-api"))
        {
            var response = await httpClient.GetAsync($"/{token.Replace("\"", "")}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.RequestMessage!.RequestUri.Should().Be(expectedUrl);
        }
    }
    
    [Fact]
    public async Task GetUrl_UrlDoesNotExists_ReturnsNotFoundStatusCode()
    {
        var httpClient = _app!.CreateHttpClient("shorty-api");
        var response   = await httpClient.GetAsync("/xxxxxxx");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}