using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Spaces;

internal class SpaceClient : ISpaceClient
{
    private readonly IGrainFactory factory;
    private readonly ICallbackRegistry callbackRegistry;
    private readonly IObserverRegistry observerRegistry;

    private ISpaceGrain Grain => factory.GetGrain<ISpaceGrain>(Guid.Empty);

    public SpaceClient(
        IGrainFactory factory,
        ICallbackRegistry callbackRegistry,
        IObserverRegistry observerRegistry)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public ObserverRef Subscribe(ISpaceObserver observer)
        => new(observerRegistry.Register(observer), observer);

    public void Unsubscribe(ObserverRef @ref)
        => observerRegistry.Deregister(@ref.Observer);

    public async Task WriteAsync(SpaceTuple tuple)
        => await Grain.WriteAsync(tuple);

    public async Task EvaluateAsync(Func<SpaceTuple> func)
        => await Grain.EvaluateAsync(LambdaSerializer.Serialize(func));

    public async ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
        => await Grain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await Grain.PeekAsync(template);

        if (tuple != null)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Register(template, callback);
        }
    }

    public async Task<SpaceTuple?> PopAsync(SpaceTemplate template)
         => await Grain.PopAsync(template);

    public async Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await Grain.PopAsync(template);

        if (tuple != null)
        {
            await callback(tuple);
        }
        else
        {
            callbackRegistry.Register(template, callback);
        }
    }

    public async ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
         => await Grain.ScanAsync(template);

    public async ValueTask<int> CountAsync()
         => await Grain.CountAsync();

    public async ValueTask<int> CountAsync(SpaceTemplate template)
         => await Grain.CountAsync(template);
}
