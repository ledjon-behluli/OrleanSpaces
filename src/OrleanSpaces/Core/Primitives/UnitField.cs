namespace OrleanSpaces.Core.Primitives;

[Serializable]
public readonly struct UnitField : IEquatable<UnitField>, IComparable<UnitField>, IComparable
{
    private static readonly UnitField _null = new();
    public static ref readonly UnitField Null => ref _null;

    public int CompareTo(UnitField other) => 0;
    int IComparable.CompareTo(object? obj) => 0;

    public bool Equals(UnitField other) => true;
    public override bool Equals(object? obj) => obj is UnitField;
    public override int GetHashCode() => 0;

    public static bool operator ==(UnitField first, UnitField second) => true;
    public static bool operator !=(UnitField first, UnitField second) => false;

    public override string ToString() => "{NULL}";
}
