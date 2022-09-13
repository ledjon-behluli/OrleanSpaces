using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel
{
    private readonly Channel<ITuple> channel = Channel.CreateUnbounded<ITuple>();

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
