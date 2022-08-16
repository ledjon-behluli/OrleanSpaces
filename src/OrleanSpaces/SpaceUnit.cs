namespace OrleanSpaces
{
    [Serializable]
    public readonly struct SpaceUnit : IEquatable<SpaceUnit>, IComparable<SpaceUnit>, IComparable
    {
        private static readonly SpaceUnit _null = new();
        public static ref readonly SpaceUnit Null => ref _null;

        public int CompareTo(SpaceUnit other) => 0;
        int IComparable.CompareTo(object? obj) => 0;

        public override int GetHashCode() => 0;

        public bool Equals(SpaceUnit other) => true;
        public override bool Equals(object? obj) => obj is SpaceUnit;

        public static bool operator ==(SpaceUnit first, SpaceUnit second) => true;
        public static bool operator !=(SpaceUnit first, SpaceUnit second) => false;

        public override string ToString() => "NULL";
    }
}
