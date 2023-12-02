using Orleans.Concurrency;
using OrleanSpaces;
using OrleanSpaces.Tuples;

namespace SiloShared;

public interface ILocalCacheGrain : IGrainWithIntegerKey
{
    ValueTask<object?> GetValue(object key);
    Task Add(object key, object value);
    Task<object?> RemoveEntry(object key);
}

[StatelessWorker]
public class LocalCacheGrain : Grain, ILocalCacheGrain
{
    private readonly ISpaceAgent agent;

    public LocalCacheGrain(ISpaceAgent agent)
    {
        this.agent = agent;
    }

    public ValueTask<object?> GetValue(object key)
    {
        var tuple = agent.Peek(new SpaceTemplate(key, null));
        return ValueTask.FromResult(tuple.IsEmpty ? null : tuple[1]);
    }

    public async Task Add(object key, object value)
    {
        await agent.WriteAsync(new SpaceTuple(key, value));
    }

    public async Task<object?> RemoveEntry(object key)
    {
        var tuple = await agent.PopAsync(new SpaceTemplate(key, null));
        return tuple.IsEmpty ? null : tuple[1];
    }
}