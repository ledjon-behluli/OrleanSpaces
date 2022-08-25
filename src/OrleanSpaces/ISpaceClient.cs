using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces;

public interface ISpaceClient
{
    ObserverRef Subscribe(ISpaceObserver observer);
    void Unsubscribe(ObserverRef @ref);

    /// <summary>
    /// <para>Used to write a tuple in the tuple space.</para>
    /// <para><i>Analogous to the "OUT" primitive in the tuple space model.</i></para>
    /// </summary>
    Task WriteAsync(SpaceTuple tuple);

    Task EvaluateAsync(Func<SpaceTuple> func);

    /// <summary>
    /// <para>Used to read a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "RD" primitive in the tuple space model.</i></para>
    /// </summary>
    ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "RDP" primitive in the tuple space model.</i></para>
    /// </summary>
    ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    /// <summary>
    /// <para>Used to read and remove a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "IN" primitive in the tuple space model.</i></para>
    /// </summary>
    Task<SpaceTuple?> PopAsync(SpaceTemplate template);

    /// <summary>
    /// <para>Used to read and remove a tuple from the tuple space.</para>
    /// <para><i>Analogous to the "INP" primitive in the tuple space model.</i></para>
    /// </summary>
    Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);

    ValueTask<int> CountAsync();
    ValueTask<int> CountAsync(SpaceTemplate template);
}