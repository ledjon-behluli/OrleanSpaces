namespace OrleanSpaces.Tuples;

internal interface IBufferConsumer<T>
    where T : unmanaged
{
    void Consume(ref Span<T> buffer, out bool? result);
}