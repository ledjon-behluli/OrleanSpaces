namespace OrleanSpaces.Tuples;

internal interface ISpanEquatable<TValue, TValueType>
    where TValue : unmanaged
    where TValueType : struct
{
    static abstract int SizeOf { get; }
    static abstract bool Equals(Span<TValue> span, TValueType left, TValueType right);
}