namespace OrleanSpaces.Tuples;

internal interface IMemoryComparer<TValue, TType>
    where TValue : unmanaged
    where TType : struct
{
    static abstract bool Equals(in TType left, ref Span<TValue> leftSpan, in TType right, ref Span<TValue> rightSpan);
}

internal interface IMemoryConsumer<T>
    where T : unmanaged
{
    void Consume(ref Span<T> memory);
}