using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel : ISpaceChannel
{
    private readonly Channel<ITuple> channel = 
        Channel.CreateUnbounded<ITuple>(new() { SingleReader = true });

    public bool HasActiveConsumer { get; set; }

    public ChannelReader<ITuple> Reader => channel.Reader;
    public ChannelWriter<ITuple> Writer => channel.Writer;
}
