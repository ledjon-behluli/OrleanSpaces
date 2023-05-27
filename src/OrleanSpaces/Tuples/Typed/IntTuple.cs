using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericTuple<int>, IEquatable<IntTuple>, IComparable<IntTuple>
{
    private readonly int[] fields;

    public ref readonly int this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<int> INumericTuple<int>.Fields => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Int);
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new ReadOnlySpan<int>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceInt
{
    public readonly int Value;
    
    internal readonly bool IsPlaceholder;

    public static readonly SpaceInt Unit = new();

    public SpaceInt()
    {
        Value = default;
        IsPlaceholder = true;
    }

    public SpaceInt(int value)
    {
        Value = value;
        IsPlaceholder = false;
    }

    public static implicit operator SpaceInt(int value) => new(value);
    public static implicit operator int(SpaceInt value) => value.Value;
}

[Immutable]
public readonly struct IntTemplate : ISpaceTemplate<int, IntTuple>
{
    private readonly SpaceInt[] fields;

    public int Length => fields.Length;

    public ref readonly int this[int index] => ref fields[index].Value;

    public IntTemplate([AllowNull] params SpaceInt[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceInt[1] { SpaceInt.Unit } : fields;

    public bool Matches(IntTuple tuple)
    {
        int[] _fields = new int[Length];
        for (int i = 0; i < Length; i++)
        {
            _fields[i] = fields[i].IsPlaceholder ? tuple[i] : this[i];
        }

        IntTuple thisTuple = new(_fields);
        return thisTuple.Equals(tuple);
    }
}
