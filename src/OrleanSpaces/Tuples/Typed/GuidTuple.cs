using Orleans.Concurrency;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct GuidTuple : ISpaceTuple<Guid, GuidTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>89b4a5d1-36ef-4106-9933-b450a587e58f</example>
    internal const int MaxFieldCharLength = 36;

    private readonly Guid[] fields;

    public ref readonly Guid this[int index] => ref fields[index];
    public int Length => fields.Length;

    public GuidTuple() : this(Array.Empty<Guid>()) { }
    public GuidTuple(params Guid[] fields) => this.fields = fields;

    public static bool operator ==(GuidTuple left, GuidTuple right) => left.Equals(right);
    public static bool operator !=(GuidTuple left, GuidTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is GuidTuple tuple && Equals(tuple);

    public bool Equals(GuidTuple other)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            if (Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < Length; i++)
            {
                // We are transforming the managed pointer(s) of type 'Guid' (obtained after re-interpreting the readonly reference(s) 'fields[i]' and 'other.fields[i]' to new mutable reference(s))
                // to new managed pointer(s) of type 'Vector128<byte>' and comparing them.

                ref Vector128<byte> vLeft = ref Helpers.CastAs<Guid, Vector128<byte>>(in fields[i]);
                ref Vector128<byte> vRight = ref Helpers.CastAs<Guid, Vector128<byte>>(in other.fields[i]);
                 
                if (vLeft != vRight)
                {
                    return false;
                }
            }

            return true;
        }

        return this.SequentialEquals(other);
    }

    public int CompareTo(GuidTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => this.TryFormat(MaxFieldCharLength, destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<Guid>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid>(fields).GetEnumerator();
}