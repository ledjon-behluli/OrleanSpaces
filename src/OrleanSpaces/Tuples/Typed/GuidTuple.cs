using Orleans.Concurrency;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct GuidTuple : ISpaceTuple<Guid, GuidTuple>
{
    private readonly Guid[] fields;

    public Guid this[int index] => fields[index];
    public int Length => fields.Length;

    public GuidTuple() : this(Array.Empty<Guid>()) { }
    public GuidTuple(Guid[] fields) => this.fields = fields;

    public static bool operator ==(GuidTuple left, GuidTuple right) => left.Equals(right);
    public static bool operator !=(GuidTuple left, GuidTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is GuidTuple tuple && Equals(tuple);

    public bool Equals(GuidTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Vector128.IsHardwareAccelerated)
        {
            for (int i = 0; i < Length; i++)
            {
                ref Vector128<byte> vLeft = ref Extensions.AsRef<Guid, Vector128<byte>>(in fields[i]);
                ref Vector128<byte> vRight = ref Extensions.AsRef<Guid, Vector128<byte>>(in other.fields[i]);

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
}