using System.Diagnostics;

namespace Shorty.API.Tests;

public class ShortUrlTokenTests
{
    [Fact]
    public void NewToken_CreatesA7CharString()
    {
        string token = ShortUrlToken.NewToken(56_800_235_584);
        
        token.Should().HaveLength(7);
    }
    
    [Fact]
    public void NewToken_CreatesAnIdempotentToken()
    {
        string token1 = ShortUrlToken.NewToken(56_800_235_586);
        string token2 = ShortUrlToken.NewToken(56_800_235_586);
        
        token1.Should().Be(token2);
    }
    
    [Theory]
    [InlineData(56_800_235_583L)]
    [InlineData(3_521_614_606_208L)]
    public void NewToken_Count_OutsideOfMaxOrMinValues_ThrowsArgumentOutOfRangeException(long count)
    {
        var act = () => ShortUrlToken.NewToken(count);

        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public void NewToken_GeneratesMoreThan5MPerSecond()
    {
        var ct = new CancellationTokenSource();
        ct.CancelAfter(TimeSpan.FromSeconds(1));

        int ops = 0;
        for (;ops < int.MaxValue; ++ops)
        {
            if (ct.IsCancellationRequested) break;
            _ = ShortUrlToken.NewToken(ops + ShortUrlToken.MinValue);
        }

        ops.Should().BeGreaterThan(5_000_000);
    }
}