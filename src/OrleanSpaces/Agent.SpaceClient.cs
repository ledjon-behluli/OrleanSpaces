using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces;

internal partial class SpaceAgent : ISpaceClient
{
    public ObserverRef Subscribe(ISpaceObserver observer)
        => new(observerRegistry.Register(observer), observer);

    public void Unsubscribe(ObserverRef @ref)
        => observerRegistry.Deregister(@ref.Observer);

    public async Task WriteAsync(SpaceTuple tuple)
        => await SpaceGrain.WriteAsync(tuple);

    public async Task EvaluateAsync(Func<SpaceTuple> func)
        => await SpaceGrain.EvaluateAsync(LambdaSerializer.Serialize(func));

    public async ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
        => await SpaceGrain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await SpaceGrain.PeekAsync(template);

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
         => await SpaceGrain.PopAsync(template);

    public async Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await SpaceGrain.PopAsync(template);

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
         => await SpaceGrain.ScanAsync(template);

    public async ValueTask<int> CountAsync()
         => await SpaceGrain.CountAsync();

    public async ValueTask<int> CountAsync(SpaceTemplate template)
         => await SpaceGrain.CountAsync(template);
}
