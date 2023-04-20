using Orleans.Concurrency;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct CharTuple : ISpaceTuple<char, CharTuple>
{
    private readonly char[] fields;

    public char this[int index] => fields[index];
    public int Length => fields.Length;

    public CharTuple() : this(Array.Empty<char>()) { }
    public CharTuple(params char[] fields) => this.fields = fields;

    public static bool operator ==(CharTuple left, CharTuple right) => left.Equals(right);
    public static bool operator !=(CharTuple left, CharTuple right) => !(left == right);

    public ReadOnlySpan<char> AsSpan() => fields.AsSpan();

    public override bool Equals(object? obj) => obj is CharTuple tuple && Equals(tuple);

    public bool Equals(CharTuple other)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            if (Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < Length; i++)
            {
                // We are loading 

                ref Vector128<byte> vLeft = ref Extensions.Transform<char, Vector128<byte>>(in fields[i]);
                ref Vector128<byte> vRight = ref Extensions.Transform<char, Vector128<byte>>(in other.fields[i]);

                if (vLeft != vRight)
                {
                    return false;
                }
            }

            return true;
        }

        return this.SequentialEquals(other);
    }

    public int CompareTo(CharTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}