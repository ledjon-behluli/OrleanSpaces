using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : IConsumable
{
    private readonly Channel<SpaceTuple> channel = 
        Channel.CreateUnbounded<SpaceTuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<SpaceTuple> Reader => channel.Reader;
    public ChannelWriter<SpaceTuple> Writer => channel.Writer;
}