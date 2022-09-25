using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : ISpaceChannel
{
    private readonly Channel<SpaceTuple> channel = 
        Channel.CreateUnbounded<SpaceTuple>(new() { SingleReader = true });

    public bool HasActiveConsumer { get; set; }

    public ChannelReader<SpaceTuple> Reader => channel.Reader;
    public ChannelWriter<SpaceTuple> Writer => channel.Writer;
}