namespace Shorty.API.Tests;

public class ShortUrlTokenTests
{
    [Fact]
    public void GetValue_CreatesA7CharString()
    {
        string token = new ShortUrlToken();

        token.Should().HaveLength(7);
    }
}