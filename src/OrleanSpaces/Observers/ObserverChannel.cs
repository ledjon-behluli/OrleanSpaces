using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : IConsumable
{
    private readonly Channel<ITuple> channel = 
        Channel.CreateUnbounded<ITuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
