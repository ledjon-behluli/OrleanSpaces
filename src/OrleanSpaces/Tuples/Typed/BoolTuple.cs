using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct BoolTuple : ISpaceTuple<bool, BoolTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>False</example>
    internal const int MaxFieldCharLength = 5;

    private readonly bool[] fields;

    public ref readonly bool this[int index] => ref fields[index];
    public int Length => fields.Length;

    public BoolTuple() : this(Array.Empty<bool>()) { }
    public BoolTuple(params bool[] fields) => this.fields = fields;

    public static bool operator ==(BoolTuple left, BoolTuple right) => left.Equals(right);
    public static bool operator !=(BoolTuple left, BoolTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is BoolTuple tuple && Equals(tuple);

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(BoolTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        // Since `bool` does not implement `ISpanFormattable` (See: https://github.com/dotnet/runtime/issues/67388),
        // we cant use `Helpers.TryFormat`, and are forced to wrap it in a struct that implements it.

        charsWritten = 0;

        int tupleLength = Length;
        int totalLength = Helpers.CalculateTotalCharLength(tupleLength, MaxFieldCharLength);

        // TODO: Benchmark & try-imporove this conversion
        SFBool[] sfBools = new SFBool[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfBools[i] = new(this[i]);
        }

        TupleFormatter<SFBool, SFBoolTuple> formatter = new(new SFBoolTuple(sfBools), MaxFieldCharLength, ref charsWritten);
        return formatter.TryFormat(totalLength, destination);
    }

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<bool>.Enumerator GetEnumerator() => new ReadOnlySpan<bool>(fields).GetEnumerator();

    readonly record struct SFBoolTuple(params SFBool[] Values) : ISpaceTuple<SFBool, SFBoolTuple>
    {
        public ref readonly SFBool this[int index] => ref Values[index];
        public int Length => Values.Length;

        public int CompareTo(SFBoolTuple other) => Length.CompareTo(other.Length);
    }

    readonly record struct SFBool(bool Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => Value.TryFormat(destination, out charsWritten);
    }
}