using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

internal partial class TupleSpace : Grain, IGrainWithGuidKey,
    ISpaceProvider, ISyncSpaceProvider, ITupleFunctionExecutor, ISpaceSubscriberRegistry
{
    private readonly SpaceObserverManager manager;
    private readonly TupleFunctionSerializer serializer;
    private readonly ILogger<TupleSpace> logger;
    private readonly IPersistentState<TupleSpaceState> space;

    public TupleSpace(
        SpaceObserverManager manager,
        TupleFunctionSerializer serializer,
        ILogger<TupleSpace> logger,
        [PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<TupleSpaceState> space)
    {
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.space = space ?? throw new ArgumentNullException(nameof(space));
    }

    #region ISpaceProvider

    public async Task WriteAsync(SpaceTuple tuple)
    {
        space.State.Tuples.Add(tuple);
        await space.WriteStateAsync();
    }

    public Task EvaluateAsync(TupleFunction @delegate) => Task.CompletedTask;

    public ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TupleResult> TryPeekAsync(SpaceTemplate template) => new(TryPeekInternal(template));

    public Task<SpaceTuple> ExtractAsync(SpaceTemplate template)
    {
        throw new NotImplementedException();
    }

    public async Task<TupleResult> TryExtractAsync(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                space.State.Tuples.Remove(tuple);
                await space.WriteStateAsync();

                return new(tuple);
            }
        }

        return TupleResult.Empty;
    }

    public ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template = default)
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

    #endregion

    #region ISyncSpaceProvider

    public TupleResult TryPeek(SpaceTemplate template) => TryPeekInternal(template);

    #endregion

    #region ITupleFunctionExecutor

    public async Task ExecuteAsync(byte[] serializedFunction)
    {
        TupleFunction? function = serializer.Deserialize(serializedFunction);
        if (function != null)
        {
            object result = function.DynamicInvoke(this);
            if (result is SpaceTuple tuple)
            {
                await WriteAsync(tuple);
            }
        }
    }

    #endregion

    #region ISubscriberRegistry

    public Task AddAsync(ISpaceObserver observer)
    {
        if (!manager.IsSubscribed(observer))
        {
            manager.Subscribe(observer);
            logger.LogInformation($"Subscribed: '{observer.GetType().FullName}'. Total number of subscribers: {manager.Count}");
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(ISpaceObserver observer)
    {
        if (manager.IsSubscribed(observer))
        {
            manager.Unsubscribe(observer);
            logger.LogInformation($"Unsubscribed: '{observer.GetType().FullName}'. Total number of subscribers: {manager.Count}");
        }

        return Task.CompletedTask;
    }

    #endregion

    private TupleResult TryPeekInternal(SpaceTemplate template)
    {
        IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

        foreach (var tuple in tuples)
        {
            if (TupleMatcher.IsMatch(tuple, template))
            {
                return new TupleResult(tuple);
            }
        }

        return TupleResult.Empty;
    }
}
