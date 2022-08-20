namespace OrleanSpaces.Core;

[Serializable]
public readonly struct NullTuple : IEquatable<NullTuple>, IComparable<NullTuple>, IComparable
{
    private static readonly NullTuple _value = new();
    public static ref readonly NullTuple Value => ref _value;

    public int CompareTo(NullTuple other) => 0;
    int IComparable.CompareTo(object? obj) => 0;

    public override int GetHashCode() => 0;

    public bool Equals(NullTuple other) => true;
    public override bool Equals(object? obj) => obj is NullTuple;

    public static bool operator ==(NullTuple first, NullTuple second) => true;
    public static bool operator !=(NullTuple first, NullTuple second) => false;

    public override string ToString() => "NULL";
}
