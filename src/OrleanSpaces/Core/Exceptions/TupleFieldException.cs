using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Core.Exceptions;

public sealed class TupleFieldException : Exception
{
    public TupleFieldException()
        : base($"Type declarations and '{nameof(NullTuple)}' are not valid '{nameof(SpaceTuple)}' fields.")
    {

    }
}