using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISpaceProvider
{
    /// <summary>
    /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task WriteAsync(SpaceTuple tuple);

    Task EvaluateAsync(TupleFunction function);

    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "RDP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "RD" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple?> TryPeekAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "INP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task<SpaceTuple> ExtractAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "IN" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task<SpaceTuple?> TryExtractAsync(SpaceTemplate template);

    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);

    ValueTask<int> CountAsync();
    ValueTask<int> CountAsync(SpaceTemplate template);
}