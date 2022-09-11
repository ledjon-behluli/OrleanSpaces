using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal class ContinuationChannel
{
    private readonly Channel<ITuple> channel = Channel.CreateUnbounded<ITuple>();

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
