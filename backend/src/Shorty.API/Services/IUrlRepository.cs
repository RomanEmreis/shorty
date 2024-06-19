namespace Shorty.API.Services;

public interface IUrlRepository
{
    Task<string> SaveAsync(string url, CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string token, CancellationToken cancellationToken = default);
}
