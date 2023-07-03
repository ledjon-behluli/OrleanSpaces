using OrleanSpaces.Tuples;

namespace OrleanSpaces;

public interface ISpaceAgentProvider
{
    /// <summary>
    /// Returns an agent that is used to interact with the tuple space.
    /// </summary>
    /// <remarks><i>Method is thread-safe.</i></remarks>
    ValueTask<ISpaceAgent> GetAsync();
}

public interface ISpaceAgentProvider<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    /// <summary>
    /// Returns an <see langword="int"/> agent that is used to interact with the strongly-typed tuple space.
    /// </summary>
    /// <remarks><i>Method is thread-safe.</i></remarks>
    ValueTask<ISpaceAgent<T, TTuple, TTemplate>> GetAsync();
}