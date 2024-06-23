namespace Shorty.API;

internal readonly struct ShortUrlToken()
{
    private const int    DefaultLength = 7;
    private const string Chars         = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string GetValue()
    {
        Span<char> token = stackalloc char[DefaultLength];

        for (int i = 0; i < token.Length; ++i)
        {
            token[i] = Chars[Random.Shared.Next(Chars.Length)];
        }

        return new string(token);
    }
}
