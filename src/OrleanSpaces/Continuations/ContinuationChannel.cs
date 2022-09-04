using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal class ContinuationChannel
{
    private readonly Channel<ISpaceElement> channel = Channel.CreateUnbounded<ISpaceElement>();

    public ChannelReader<ISpaceElement> Reader => channel.Reader;
    public ChannelWriter<ISpaceElement> Writer => channel.Writer;
}
