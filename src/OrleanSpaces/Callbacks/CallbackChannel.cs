using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel<T> : IConsumable
    where T : ISpaceTuple
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<T> channel =
        Channel.CreateUnbounded<T>(new() { SingleReader = true });

    public ChannelReader<T> Reader => channel.Reader;
    public ChannelWriter<T> Writer => channel.Writer;
}