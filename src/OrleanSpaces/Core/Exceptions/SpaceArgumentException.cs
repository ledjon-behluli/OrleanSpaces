namespace OrleanSpaces.Core.Exceptions;

public sealed class SpaceArgumentException : ArgumentException
{
    public SpaceArgumentException(string methodName)
        : base("An invalid type or number of arguments has been passed to space method call.", methodName)
    {

    }
}
