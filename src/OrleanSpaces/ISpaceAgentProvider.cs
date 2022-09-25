namespace OrleanSpaces;

public interface ISpaceAgentProvider
{
    /// <summary>
    /// Returns an <see cref="ISpaceAgent"/> that is used to interact with the tuple space.
    /// </summary>
    /// <remarks><i>Method is thread-safe.</i></remarks>
    ValueTask<ISpaceAgent> GetAsync();
}
