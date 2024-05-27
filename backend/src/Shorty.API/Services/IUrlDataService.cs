namespace Shorty.API.Services;

public interface IUrlDataService
{
    Task<string> SaveAsync(string url, CancellationToken cancellationToken = default);
    Task<string> GetAsync(string token, CancellationToken cancellationToken = default);
}
