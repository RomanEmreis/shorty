namespace Shorty.API;

public readonly struct ShortUrlToken
{
    internal const long      
        MinValue = 56_800_235_584L, 
        MaxValue = 3_521_614_606_207L; 
    
    private const int    
        DefaultLength = 7, 
        Base = 62;
    
    private const string Chars = "QoNPMlEDkABC06789zxyvwustrq21453pOnmLKjZYXWVUTSRihgfedcbJIHGFa";
    
    private readonly string _value;

    private ShortUrlToken(long count)
    {
        Span<char> token = stackalloc char[DefaultLength];

        var j = DefaultLength;
        while (count != 0)
        {
            int i = (int) (count % Base);
            token[--j] = Chars[i];
            count /= Base;
        }

        _value = new string(token);
    }

    /// <summary>
    ///     Idempotent creates a unique 7-character token based on the provided count value.
    ///     Passing the same count value leads to the same result.
    ///     <br/>
    ///     The overall number of available unique tokens is 3 464 814 370 623,
    ///     for the greater number of tokens the length should be increased to 8 e.t.c
    /// </summary>
    /// <param name="count">A value that a token will be based on</param>
    /// <returns>
    ///     A 7-character token based on the provided count value.
    /// </returns>
    internal static ShortUrlToken NewToken(long count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, MinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxValue);
        
        return new ShortUrlToken(count);
    }

    public override string ToString() => _value;

    public static implicit operator string(ShortUrlToken token) => token._value;
}
