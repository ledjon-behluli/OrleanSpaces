using Orleans.Streams;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Helpers;

internal static class OrleansHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task SubscribeOrResumeAsync<T>(this IAsyncStream<T> stream, IAsyncObserver<T> observer)
    {
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
