using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct GuidTuple : ISpaceTuple<Guid>, IEquatable<GuidTuple>, IComparable<GuidTuple>
{
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
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector128.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
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

    public int CompareTo(GuidTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Guid);

    public ReadOnlySpan<Guid>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceGuid
{
    public readonly Guid Value;

    internal static readonly SpaceGuid Default = new();

    public SpaceGuid(Guid value) => Value = value;

    public static implicit operator SpaceGuid(Guid value) => new(value);
    public static implicit operator Guid(SpaceGuid value) => value.Value;
}

[Immutable]
public readonly struct GuidTemplate : ISpaceTemplate<GuidTuple>
{
    private readonly SpaceGuid[] fields;

    public GuidTemplate([AllowNull] params SpaceGuid[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceGuid[1] { new SpaceUnit() } : fields;
}
