namespace OrleanSpaces.Tuples;

internal interface ISpanEquatable<TValue, TValueType>
    where TValue : unmanaged
    where TValueType : struct
{
    abstract static int SizeOf { get; }
    abstract static bool Equals(Span<TValue> span, TValueType left, TValueType right);
}