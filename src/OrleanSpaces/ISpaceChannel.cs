namespace OrleanSpaces;

public interface ISpaceChannel
{
    /// <remarks>Method is thread-safe.</remarks>
    Task<ISpaceAgent> GetAsync();
}

