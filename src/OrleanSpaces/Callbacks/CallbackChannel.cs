using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal class CallbackChannel
{
    private readonly Channel<SpaceTuple> channel = Channel.CreateUnbounded<SpaceTuple>();

    public ChannelReader<SpaceTuple> Reader => channel.Reader;
    public ChannelWriter<SpaceTuple> Writer => channel.Writer;
}