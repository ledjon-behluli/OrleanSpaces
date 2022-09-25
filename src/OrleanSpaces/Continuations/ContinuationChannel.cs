using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel : IConsumable
{
    private readonly Channel<ITuple> channel = 
        Channel.CreateUnbounded<ITuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
