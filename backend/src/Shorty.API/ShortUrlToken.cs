namespace Shorty.API;

internal readonly struct ShortUrlToken(string url)
{
    private const int    DefaultLength = 5;
    private const string Chars         = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string GetValue()
    {
        Random random = new();
        Span<char> token = stackalloc char[DefaultLength];

        for (int i = 0; i < token.Length; ++i)
        {
            token[i] = Chars[random.Next(Chars.Length)];
        }

        return new string(token);
    }
}
