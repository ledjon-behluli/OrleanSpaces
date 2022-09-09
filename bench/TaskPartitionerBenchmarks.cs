using BenchmarkDotNet.Attributes;
using OrleanSpaces.Utils;

[MemoryDiagnoser]
[ShortRunJob]
public class TaskPartitionerBenchmarks
{
    [Params(128, 256, 1024)]//, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384)]
    public int ItemsCount { get; set; }

    [Benchmark]
    public async Task Task_WhenAll()
    {
        Item[] items = GetItems();
        List<Task> tasks = new();

        foreach (Item item in items)
            tasks.Add(Task.Run(() => item.PingAsync()));

        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task Partitioner_WhenAll()
    {
        Item[] items = GetItems();
        await TaskPartitioner.WhenAll(items, async item => await item.PingAsync());
    }

    private Item[] GetItems()
    {
        var items = new Item[ItemsCount];

        for (int i = 0; i < ItemsCount; i++)
            items[i] = new();

        return items;
    }

    private class Item
    {
        public async Task PingAsync()
        {
            await Task.Delay(1000);
        }
    }
}