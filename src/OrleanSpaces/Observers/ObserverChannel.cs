using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : IConsumable
{
    private readonly Channel<ISpaceTuple> channel = 
        Channel.CreateUnbounded<ISpaceTuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<ISpaceTuple> Reader => channel.Reader;
    public ChannelWriter<ISpaceTuple> Writer => channel.Writer;
}
