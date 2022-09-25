using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : ISpaceChannel
{
    private readonly Channel<ITuple> channel = 
        Channel.CreateUnbounded<ITuple>(new() { SingleReader = true });

    public bool HasActiveConsumer { get; set; }

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
