namespace OrleanSpaces.Tuples;

internal interface ITupleComparer<TValue, TValueType>
    where TValue : unmanaged
    where TValueType : struct
{
    static abstract bool Equals(TValueType left, Span<TValue> leftSpan, TValueType right, Span<TValue> rightSpan);
}