using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<ISpaceTuple> channel = 
        Channel.CreateUnbounded<ISpaceTuple>(new() { SingleReader = true });

    public ChannelReader<ISpaceTuple> Reader => channel.Reader;
    public ChannelWriter<ISpaceTuple> Writer => channel.Writer;
}