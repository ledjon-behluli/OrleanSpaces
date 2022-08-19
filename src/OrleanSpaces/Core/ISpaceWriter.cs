using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Core;

public interface ISpaceWriter
{
    /// <summary>
    /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ISpaceGrain"/>.</para>
    /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
    /// </summary>
    Task WriteAsync(SpaceTuple tuple);

    Task EvaluateAsync(Func<SpaceTuple> func);

    internal Task EvaluateAsync(byte[] serializedFunc);
}
