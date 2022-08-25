using Orleans;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : IGrainWithGuidKey
{
    ValueTask<Guid> ConnectAsync();

    /// <summary>
    /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task WriteAsync(SpaceTuple tuple);

    Task EvaluateAsync(byte[] serializedFunc);

    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "RD" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "IN" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task<SpaceTuple?> PopAsync(SpaceTemplate template);

    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);

    ValueTask<int> CountAsync();
    ValueTask<int> CountAsync(SpaceTemplate template);
}
