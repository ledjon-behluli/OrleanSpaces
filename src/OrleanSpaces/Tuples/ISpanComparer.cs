namespace OrleanSpaces.Tuples;

internal interface ISpanEquatable<TValue, TValueType>
    where TValue : unmanaged
    where TValueType : struct
{
    static abstract bool Equals(TValueType left, Span<TValue> leftSpan, TValueType right, Span<TValue> rightSpan);
}