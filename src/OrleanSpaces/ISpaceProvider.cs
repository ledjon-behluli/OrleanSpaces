using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISpaceProvider
{
    /// <summary>
    /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task Write(SpaceTuple tuple);

    Task Evaluate(TupleFunction function);

    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "RDP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple> Peek(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "RD" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask<TupleResult> TryPeek(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "INP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task<SpaceTuple> Extract(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceProvider"/>.</para>
    /// <para><i>Analogous to the "IN" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task<TupleResult> TryExtract(SpaceTemplate template);

    IEnumerable<SpaceTuple> Scan(SpaceTemplate template);

    ValueTask<int> Count();
    ValueTask<int> Count(SpaceTemplate template);
}