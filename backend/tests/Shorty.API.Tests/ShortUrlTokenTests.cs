namespace Shorty.API.Tests;

public class ShortUrlTokenTests
{
    [Fact]
    public void GetValue_CreatesA7CharString()
    {
        var token = new ShortUrlToken();

        var value = token.GetValue();

        value.Should().HaveLength(7);
    }
}