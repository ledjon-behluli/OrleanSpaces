namespace OrleanSpaces.Tuples;

public interface ISpaceTuple<T, H> : IEquatable<H>, IComparable<H>
    where T : notnull
    where H : ISpaceTuple<T, H>
{
    T this[int index] { get; }
    int Length { get; }
}