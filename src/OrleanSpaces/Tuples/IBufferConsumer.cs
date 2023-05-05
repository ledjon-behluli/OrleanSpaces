namespace OrleanSpaces.Tuples;

internal interface IBufferConsumer<T, TResult>
    where T : unmanaged
    where TResult : struct
{
    TResult Consume(ref Span<T> buffer);
}

internal interface IBufferBooleanResultConsumer<T> : IBufferConsumer<T, bool>
    where T : unmanaged
{

}

internal interface IBufferUnitResultConsumer<T> : IBufferConsumer<T, SpaceUnit>
    where T : unmanaged
{

}

