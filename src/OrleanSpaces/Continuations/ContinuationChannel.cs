using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal class ContinuationChannel
{
    private readonly Channel<ISpaceTuple> channel = Channel.CreateUnbounded<ISpaceTuple>();

    public ChannelReader<ISpaceTuple> Reader => channel.Reader;
    public ChannelWriter<ISpaceTuple> Writer => channel.Writer;
}
