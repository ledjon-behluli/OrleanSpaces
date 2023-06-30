using Orleans.Streams;
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Helpers;

internal static class OrleansHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAsyncStream<TupleAction<T>> GetStream<T, TGrain>(this Grain grain)
        where T : ISpaceTuple
        where TGrain : IBaseGrain<T>
        => grain.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<T>>(TGrain.StreamId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task SubscribeAsync<T, TGrain>(this IAsyncObserver<TupleAction<T>> observer, IClusterClient client) 
        where T : ISpaceTuple
        where TGrain : IBaseGrain<T>
    {
        var stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<T>>(TGrain.StreamId);
        var handles = await stream.GetAllSubscriptionHandles();

        if (handles.Count > 0)
        {
            await handles[0].ResumeAsync(observer);
        }
        else
        {
            await stream.SubscribeAsync(observer);
        }
    }
}
