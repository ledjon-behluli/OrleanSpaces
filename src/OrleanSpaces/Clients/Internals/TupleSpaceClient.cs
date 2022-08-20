using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Clients.Internals;

internal class TupleSpaceClient : ITupleSpaceClient
{
    private readonly LambdaSerializer serializer;
    private readonly ICallbackRegistry registry;
    private readonly IGrainFactory factory;

    private ISpaceGrain Grain => factory.GetSpaceGrain();

    public TupleSpaceClient(
        LambdaSerializer serializer,
        ICallbackRegistry registry,
        IGrainFactory factory)
    {
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task WriteAsync(SpaceTuple tuple)
        => await Grain.WriteAsync(tuple);

    public async Task EvaluateAsync(Func<SpaceTuple> func)
        => await Grain.EvaluateAsync(serializer.Serialize(func));

    public async ValueTask<SpaceTuple?> PeekAsync(SpaceTemplate template)
        => await Grain.PeekAsync(template);

    public async ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await Grain.PeekAsync(template);

        if (tuple != null)
        {
            await callback((SpaceTuple)tuple);
        }
        else
        {
            registry.Register(template, callback);
        }
    }

    public async Task<SpaceTuple?> PopAsync(SpaceTemplate template)
         => await Grain.PopAsync(template);

    public async Task PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        SpaceTuple? tuple = await Grain.PopAsync(template);

        if (tuple != null)
        {
            await callback((SpaceTuple)tuple);
        }
        else
        {
            registry.Register(template, callback);
        }
    }

    public async ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template)
         => await Grain.ScanAsync(template);

    public async ValueTask<int> CountAsync()
         => await Grain.CountAsync();

    public async ValueTask<int> CountAsync(SpaceTemplate template)
         => await Grain.CountAsync(template);
}
