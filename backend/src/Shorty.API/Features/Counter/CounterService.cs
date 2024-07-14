using StackExchange.Redis;

namespace Shorty.API.Features.Counter;

public interface ICounterService
{
    Task<long> IncrementAsync();
}

internal sealed class CounterService(IConnectionMultiplexer connectionMultiplexer) : ICounterService
{
    private const string CounterKey = "shorty_counter";

    private readonly IDatabase _redis = connectionMultiplexer.GetDatabase();
    
    public async Task<long> IncrementAsync()
    {
        const long supplement = ShortUrlToken.MinValue;

        var value = await _redis.StringIncrementAsync(CounterKey);
        return value + supplement;
    }
}