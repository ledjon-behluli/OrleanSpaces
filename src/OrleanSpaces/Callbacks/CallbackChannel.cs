using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<SpaceTuple> channel = 
        Channel.CreateUnbounded<SpaceTuple>(new() { SingleReader = true });

    public ChannelReader<SpaceTuple> Reader => channel.Reader;
    public ChannelWriter<SpaceTuple> Writer => channel.Writer;
}

internal sealed class CallbackChannel<T, TTuple> : IConsumable
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<TTuple> channel =
        Channel.CreateUnbounded<TTuple>(new() { SingleReader = true });

    public ChannelReader<TTuple> Reader => channel.Reader;
    public ChannelWriter<TTuple> Writer => channel.Writer;
}