namespace Shorty.API;

public readonly struct ShortUrlToken
{
    private const    int    DefaultLength = 7;
    private const    string Chars         = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private readonly string _value;

    public ShortUrlToken()
    {
        Span<char> token = stackalloc char[DefaultLength];

        for (int i = 0; i < token.Length; ++i)
        {
            token[i] = Chars[Random.Shared.Next(Chars.Length)];
        }

        _value = new string(token);
    }

    internal static ShortUrlToken NewToken() => new();

    public static implicit operator string(ShortUrlToken token) => token._value;
}
