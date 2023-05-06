namespace OrleanSpaces.Tuples;

internal interface IBufferConsumer<T>
    where T : unmanaged
{
    bool Consume(ref Span<T> buffer);
}