using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Core;

public interface ISpaceBlockingReader
{
    /// <summary>
    /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "RDP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    ValueTask PeekAsync(SpaceTemplate template, Action<SpaceTuple> callback);

    /// <summary>
    /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "INP" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task PopAsync(SpaceTemplate template, Action<SpaceTuple> callback);
}
