using OrleanSpaces.Agents;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces;

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

internal class AgentProvider<T, TTuple, TTemplate> : ISpaceAgentProvider<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ISpaceAgent<T, TTuple, TTemplate> agent;
    private bool initialized;

    public AgentProvider(ISpaceAgent<T, TTuple, TTemplate> agent)
    {
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent<T, TTuple, TTemplate>> GetAsync()
    {
        if (initialized)
        {
            return agent;
        }

        await semaphore.WaitAsync();

        try
        {
            await agent.InitializeAsync();
            initialized = true;
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}

internal sealed class IntAgentProvider : AgentProvider<int, IntTuple, IntTemplate>
{
    public IntAgentProvider(IntAgent agent) : base(agent) { }
}