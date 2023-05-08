using OrleanSpaces.Tuples.Typed;
using System.Runtime.CompilerServices;

#region Test

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

//Console.WriteLine($"{new IntTuple(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)}");

Console.WriteLine($"{new IntTuple(
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)}");

//Console.WriteLine($"{new IntTuple(1, 10, 100)}");
//Console.WriteLine($"{new BoolTuple(true, false, true)}");
//Console.WriteLine($"{new CharTuple('a', 'b', 'c')}");

//Console.WriteLine($"{new StringTuple()}");
//Console.WriteLine($"{new StringTuple("a")}");
//Console.WriteLine($"{new StringTuple("ab")}");
//Console.WriteLine($"{new StringTuple("a", "ab")}");
//Console.WriteLine($"{new StringTuple("a", "ab", "abc")}");
//Console.WriteLine($"{new StringTuple("a", "ab", "abc", "abcd")}");

#endregion

byte[] test1 = new byte[1] { 0x55 };
ReadOnlySpan<byte> test2 = new byte[1] { 0x56 };
Span<byte> test3 = new byte[128];
test3.Fill(0x55);

Span<char> buffer = stackalloc char[64];
WriteLine(buffer, $"test1:{test1:S}"); // "test1:VQ=="
WriteLine(buffer, $"test2:{test2:S}"); // "test2:Vg=="
WriteLine(buffer, $"test3:{test3:S}"); // "test3:" (the buffer is not big enough)

static void WriteLine(
    Span<char> destination,
    [InterpolatedStringHandlerArgument("destination")] ref MyReallyBadAndUntestedInterpolatedStringHandler builder)
{
    Console.WriteLine(builder.ToStringAndClear());
}

Console.ReadKey();

[InterpolatedStringHandler]
public ref struct MyReallyBadAndUntestedInterpolatedStringHandler
{
    private readonly Span<char> _buffer;
    private int _charsWritten;

    public MyReallyBadAndUntestedInterpolatedStringHandler(int literalLength, int formattedCount, Span<char> destination)
    {
        _buffer = destination;
        _charsWritten = 0;
    }

    public bool AppendLiteral(string s)
    {
        if (s.AsSpan().TryCopyTo(_buffer.Slice(_charsWritten)))
        {
            _charsWritten += s.Length;
            return true;
        }

        return false;
    }

    public bool AppendFormatted<T>(T t, ReadOnlySpan<char> format)
    {
        Span<char> buffer = _buffer.Slice(_charsWritten);

        if (t is ISpanFormattable formattable)
        {
            bool success = formattable.TryFormat(buffer, out int charsWritten, format, null);
            _charsWritten += charsWritten;
            return success;
        }

        string s = t?.ToString() ?? "";

        if (s.AsSpan().TryCopyTo(buffer))
        {
            _charsWritten += s.Length;
            return true;
        }

        return false;
    }

    // This is necessary otherwise byte[] arg resolves to the generic overload
    public bool AppendFormatted(byte[] bytes, ReadOnlySpan<char> format)
        => AppendFormatted((ReadOnlySpan<byte>)bytes, format);

    public bool AppendFormatted(ReadOnlySpan<byte> bytes, ReadOnlySpan<char> format)
    {
        if (format != "S")
        {
            throw new ArgumentException("Invalid format");
        }

        bool success = Convert.TryToBase64Chars(bytes, _buffer.Slice(_charsWritten), out int charsWritten);
        _charsWritten += charsWritten;
        return success;
    }

    public string ToStringAndClear()
    {
        string result = new string(_buffer.Slice(0, _charsWritten));
        _buffer.Clear();
        _charsWritten = 0;
        return result;
    }
}