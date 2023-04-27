using Newtonsoft.Json.Linq;
using Orleans.Concurrency;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct GuidTuple : IValueTuple<Guid, GuidTuple>, ISpanFormattable
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

                Vector128<byte> vLeft = AsVector(in fields[i]);
                Vector128<byte> vRight = AsVector(in other.fields[i]);
                 
                if (vLeft != vRight)
                {
                    return false;
                }
            }

            return true;
        }

        return this.SequentialEquals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<byte> AsVector(in Guid value) // 'value' is passed using 'in' to avoid defensive copying.
       => Unsafe.As<Guid, Vector128<byte>>(ref Unsafe.AsRef(in value));

    public int CompareTo(GuidTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(MaxFieldCharLength, destination, out charsWritten);

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<Guid>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid>(fields).GetEnumerator();
}