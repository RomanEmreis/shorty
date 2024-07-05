namespace Shorty.AppHost.Tests;

public abstract class ShortyAppHostFactory : IAsyncLifetime
{
    private protected DistributedApplication? _app;
    private IDistributedApplicationTestingBuilder? _appHost;
    
    public async Task InitializeAsync()
    {
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shorty_AppHost>();
        _app = await _appHost.BuildAsync();
        
        await _app.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _app!.StopAsync();
        await _app!.DisposeAsync();
    }
}