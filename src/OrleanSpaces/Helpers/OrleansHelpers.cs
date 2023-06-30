using Orleans.Streams;
using OrleanSpaces.Tuples;
using Orleans.Runtime;

namespace OrleanSpaces.Helpers;

internal static class OrleansHelpers
{
    public static IAsyncStream<TupleAction<T>> GetStream<T>(this Grain grain,StreamId streamId)
        where T : ISpaceTuple
        => grain.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<T>>(streamId);

    public static async Task SubscribeAsync<T>(this IClusterClient client, IAsyncObserver<TupleAction<T>> observer, StreamId streamId) 
        where T : ISpaceTuple
    {
        var stream = client.GetStreamProvider(Constants.PubSubProvider).GetStream<TupleAction<T>>(streamId);
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
