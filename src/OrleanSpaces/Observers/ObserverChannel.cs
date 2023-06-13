using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<ISpaceElement> channel = 
        Channel.CreateUnbounded<ISpaceElement>(new() { SingleReader = true });

    public ChannelReader<ISpaceElement> Reader => channel.Reader;
    public ChannelWriter<ISpaceElement> Writer => channel.Writer;
}