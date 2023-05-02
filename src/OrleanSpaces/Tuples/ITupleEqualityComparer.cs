namespace OrleanSpaces.Tuples;

internal interface ITupleEqualityComparer<T, TTuple>
    where T : unmanaged
    where TTuple : struct
{
    static abstract bool Equals(TTuple left, Span<T> leftSpan, TTuple right, Span<T> rightSpan);
}