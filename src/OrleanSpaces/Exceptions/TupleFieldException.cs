using OrleanSpaces.Types;
namespace OrleanSpaces.Exceptions;

public sealed class TupleFieldException : Exception
{
    public TupleFieldException()
        : base($"Type declarations and '{nameof(NullTuple)}' are not valid '{nameof(SpaceTuple)}' fields.")
    {

    }
}
