using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using System.Threading.Tasks;

namespace OrleanSpaces.Hosts.Grains;

[Serializable]
internal class SpaceState
{
    public List<SpaceTuple> Tuples { get; set; } = new();
}

internal class SpaceGrain : Grain, ISpaceGrain
{
    private readonly ObserverManager manager;
    private readonly ILogger<SpaceGrain> logger;
    private readonly IPersistentState<SpaceState> space;

    public SpaceGrain(
        ILogger<SpaceGrain> logger,
        [PersistentState("tupleSpace")] IPersistentState<SpaceState> space)
    {
        this.manager = new ObserverManager();
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    ValueTask<bool> ISpaceObserverRegistry.IsRegisteredAsync(ISpaceObserver observer)
        => new(manager.Observers.Any(x => x.Equals(observer)));

    ValueTask ISpaceObserverRegistry.RegisterAsync(ISpaceObserver observer)
    {
        if (manager.TryAdd(observer))
        {
            logger.LogInformation("Observer Registration - Current number of observers: {ObserversCount}", manager.Count);
        }

        return new();
    }

    ValueTask ISpaceObserverRegistry.DeregisterAsync(ISpaceObserver observer)
    {
        if (manager.TryRemove(observer))
        {
            logger.LogInformation("Observer Deregistration - Current number of observers: {ObserversCount}", manager.Count);
        }

        return new();
    }

    public async Task WriteAsync(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();

        manager.Broadcast(observer => observer.ReceiveAsync(tuple).Ignore());
    }

    public async Task EvaluateAsync(byte[] serializedFunc)
    {
        Func<SpaceTuple> function = LambdaSerializer.Deserialize(serializedFunc);
        object result = function.DynamicInvoke();

        if (result is SpaceTuple tuple)
        {
            await WriteAsync(tuple);
        }
    }

    public ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new(tuple);
            }
        }

        return default;
    }

    public async Task<SpaceTuple?> PopAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                space.State.Tuples.Remove(tuple);
                await space.WriteStateAsync();

                return tuple;
            }
        }

        return default;
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
    {
        List<SpaceTuple> results = new();
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                results.Add(tuple);
            }
        }

        return new(results);
    }

    public ValueTask<int> CountAsync() => new(space.State.Tuples.Count);

    public ValueTask<int> CountAsync(SpaceTemplate template) =>
        new(space.State.Tuples.Count(sp => sp.Length == template.Length && TupleMatcher.IsMatch(sp, template)));
}
