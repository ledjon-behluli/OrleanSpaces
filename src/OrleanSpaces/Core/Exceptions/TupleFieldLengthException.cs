namespace OrleanSpaces.Core.Exceptions;

public sealed class TupleFieldLengthException : Exception
{
    public TupleFieldLengthException(string typeName)
        : base($"Construction of '{typeName}' without any fields is not allowed.")
    {

    }
}