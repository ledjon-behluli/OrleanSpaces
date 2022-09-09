using System.Collections.Concurrent;

namespace OrleanSpaces.Utils;

internal static class TaskPartitioner
{
    internal static Task WhenAll<T>(IEnumerable<T> source, Func<T, Task> func)
    {
        return Task.WhenAll(
            Partitioner
                .Create(source)
                .GetPartitions(Environment.ProcessorCount)
                .AsParallel()
                .Select(p => AwaitPartition(p)));

        async Task AwaitPartition(IEnumerator<T> partition)
        {
            using (partition)
            {
                while (partition.MoveNext())
                {
                    await Task.Yield(); // prevents a sync thread hangup
                    await func(partition.Current);
                }
            }
        }
    }
}